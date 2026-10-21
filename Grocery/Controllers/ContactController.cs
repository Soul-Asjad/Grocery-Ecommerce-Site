using Grocery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grocery.Controllers
{
    public class ContactController : Controller
    {
        private readonly GroceryDBContext context;

        public ContactController(GroceryDBContext context)
        {
            this.context = context;
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(Contact data)
        {
            if (HttpContext.Session.GetString("user_role") != "User")
            {
                return RedirectToAction("Login", "User");
            }

            var session = HttpContext.Session.GetInt32("user_id");

            if (session == null)
            {
                return RedirectToAction("Login", "User");
            }

            this.context.Contact.Add(data);
            this.context.SaveChanges();

            return View();
        }

        public async Task<IActionResult> ContactMessages()
        {
            if (HttpContext.Session.GetString("user_role") != "Admin")
            {
                return RedirectToAction("Login", "User");
            }

            var messages = await this.context.Contact.AsNoTracking().ToListAsync();
            return View(messages);
        }

        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("user_role") != "Admin")
            {
                return RedirectToAction("Login", "User");
            }

            var data = this.context.Contact.FirstOrDefault(x => x.ContactID == id);
            return View(data);
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("user_role") != "Admin")
            {
                return RedirectToAction("Login", "User");
            }

            var data = this.context.Contact.FirstOrDefault(x => x.ContactID == id);
            this.context.Contact.Remove(data);
            this.context.SaveChanges();

            return RedirectToAction("ContactMessages");
        }
    }
}
