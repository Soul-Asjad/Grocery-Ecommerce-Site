using Grocery.Models;
using Microsoft.AspNetCore.Mvc;

namespace Grocery.Controllers
{
    public class AdminController : Controller
    {
        private readonly GroceryDBContext context;

        public AdminController(GroceryDBContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            ViewBag.products = this.context.Products.Count();
            ViewBag.users = this.context.User.Count();
            ViewBag.Orders = this.context.Orders.Count();
            ViewBag.TotalEarn = this.context.Orders.Sum(x => x.TotalAmount);


            return View();
        }

        public IActionResult Categories()
        {
            return View();
        }





    }
}
