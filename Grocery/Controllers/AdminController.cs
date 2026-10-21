using Grocery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            if(HttpContext.Session.GetString("user_role") != "Admin")
            {
                return RedirectToAction("Login" , "User");
            }


            ViewBag.products = this.context.Products.Count();
            ViewBag.users = this.context.User.Count();
            ViewBag.Orders = this.context.Orders.Count();
            ViewBag.OrderList = this.context.Orders.Include(x => x.users).ToList();
            ViewBag.TotalEarn = this.context.Orders.Sum(x => x.TotalAmount);
            ViewBag.pendingOrders = this.context.Orders.Count(x => x.Orderstatus == "Pending");
            ViewBag.ConfirmedOrders = this.context.Orders.Count(x => x.Orderstatus == "Confirmed");
            ViewBag.InProgressOrders = this.context.Orders.Count(x => x.Orderstatus == "In_Progress");
            ViewBag.CompeleteOrders = this.context.Orders.Count(x => x.Orderstatus == "Delivered");
            ViewBag.CancelOrders = this.context.Orders.Count(x => x.Orderstatus == "Canceled");
            ViewBag.ReturnOrders = this.context.Orders.Count(x => x.Orderstatus == "Returned");


            return View();
        }

    }
}
