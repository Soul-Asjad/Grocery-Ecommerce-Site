using Grocery.Models;
using Microsoft.AspNetCore.Mvc;

namespace Grocery.Controllers
{
    public class OrderController : Controller
    {
        private readonly GroceryDBContext context;

        public OrderController(GroceryDBContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var session = HttpContext.Session.GetInt32("user_id");

            var order = this.context.Orders.Where(x => x.UserID == session).ToList();

            return View(order);
        }

        public IActionResult Details(int id)
        {
            var order = this.context.Orders.FirstOrDefault(x => x.OrderID == id);
            if(order == null)
            {
                return NotFound();
            }

            var detail = this.context.OrderItems.Where(x => x.OrderID == order.OrderID).ToList();

            return View(detail);
        }

        public IActionResult View_Orders()
        {
            var orders = this.context.Orders.ToList();

            return View(orders);
        }

        public IActionResult Delete(int id)
        {
            var order = this.context.Orders.FirstOrDefault(x => x.OrderID == id);

            this.context.Orders.Remove(order);
            this.context.SaveChanges();

            return RedirectToAction("View_Orders", "Order");
        }
    }
}
