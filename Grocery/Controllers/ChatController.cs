using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Grocery.Models;
using Microsoft.AspNetCore.Mvc;

namespace Grocery.Controllers
{
    public class ChatTurn
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
        public List<ChatTurn> History { get; set; }
    }

    public class ChatController : Controller
    {
        private readonly GroceryDBContext context;
        private readonly IHttpClientFactory httpFactory;
        private readonly IConfiguration config;

        public ChatController(GroceryDBContext context, IHttpClientFactory httpFactory, IConfiguration config)
        {
            this.context = context;
            this.httpFactory = httpFactory;
            this.config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] ChatRequest req)
        {
            if (string.IsNullOrWhiteSpace(req?.Message) || req.Message.Length > 300)
                return Json(new { reply = "Please type a message under 300 characters." });

            // Keep only the last 6 earlier messages, and only the roles user/assistant
            var previous = (req.History ?? new List<ChatTurn>())
                .Where(h => (h.Role == "user" || h.Role == "assistant") && !string.IsNullOrWhiteSpace(h.Content))
                .TakeLast(6)
                .Select(h => new { role = h.Role, content = h.Content.Length > 500 ? h.Content.Substring(0, 500) : h.Content })
                .ToList();

            // 1. Find products using the new message and the last earlier user message
            string searchText = req.Message + " " + (previous.LastOrDefault(h => h.role == "user")?.content ?? "");

            var words = searchText.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                  .Where(w => w.Length > 2).Distinct().ToList();

            var products = this.context.Products.Where(p => p.Isactive).ToList()
                .Where(p => words.Any(w => p.ProductName.ToLower().Contains(w)))
                .Take(5).ToList();

            string productText = products.Any()
    ? string.Join("\n", products.Select(p =>
        $"- Name: {p.ProductName} | Description: {(p.Description != null && p.Description.Length > 300 ? p.Description.Substring(0, 300) : p.Description)} | Price: ${p.Price:0.00} per {p.Unit} | Stock: {p.StockQuantity}"))
    : "No matching products found. Examples of products we sell: " +
      string.Join(", ", this.context.Products.Where(p => p.Isactive).Select(p => p.ProductName).Take(8).ToList());

            // 2. Instructions for the model
            string system =
                "You are Grocery Assistant, the virtual assistant of an online grocery store. " +
                "Be professional, friendly and brief: normally one to three short sentences.\n" +
                "Rules:\n" +
                "- Reply in plain text only. No markdown, no asterisks, no emojis, and no lists unless the customer asks for a list.\n" +
                "- Answer only what the customer asked. Do not add extra information they did not ask for.\n" +
                "- If the customer only greets you, reply with a short greeting and ask how you can help. Say nothing else.\n" +
                "- If the customer asks about a product or says 'tell me about' a product, give only a short description from its Description. " +
                "Mention the price, unit or stock only if the customer asks for them.\n" +
                "- Use only the product information below. Never invent details. If a product is not listed, say you could not find it and ask for the product name.\n" +
                "- If the customer asks for a product description without naming a product, ask which product they mean and you may suggest a few examples.\n" +
                "- Mention logging in, My Orders or the Contact page only when the customer asks about ordering, the cart, their account, cancelling or changing an order, or contacting the store.\n" +
                "Store policies, to use only when relevant: customers must log in to add items to the cart, check out or send a message. " +
                "Users can see their orders under My Orders but cannot cancel or change them there; " +
                "for cancellations or changes they should send a message on the Contact page.\n" +
                "Product information:\n" + productText;

            // 3. Build the message list: instructions, earlier chat, new question
            var messages = new List<object> { new { role = "system", content = system } };
            messages.AddRange(previous.Cast<object>());
            messages.Add(new { role = "user", content = req.Message });

            var body = new
            {
                model = config["Chat:Model"],
                temperature = 0.3,
                messages = messages
            };

            var msg = new HttpRequestMessage(HttpMethod.Post, "https://api.groq.com/openai/v1/chat/completions");
            msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config["Chat:ApiKey"]);
            msg.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var res = await httpFactory.CreateClient().SendAsync(msg);

            if (!res.IsSuccessStatusCode)
            {
                Console.WriteLine(await res.Content.ReadAsStringAsync());
                return Json(new { reply = "Sorry, the assistant is busy. Please try again or use the Contact page." });
            }

            using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
            var reply = doc.RootElement.GetProperty("choices")[0]
                           .GetProperty("message").GetProperty("content").GetString();

            return Json(new { reply });
        }
    }
}