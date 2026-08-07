using Grocery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grocery.Controllers
{
    public class HomeController : Controller
    {
        private readonly GroceryDBContext context;

        public HomeController(GroceryDBContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            ViewBag.category = this.context.Categories.ToList();
            ViewBag.products = this.context.Products.ToList();

            //ViewBag.Category = category;
            //ViewBag.Products = products;

            return View();
        }


        public IActionResult ProductDetails(int id)
        {
            var data = this.context.Products.Include(x => x.categories).FirstOrDefault(x => x.ProductID == id);

            if (data == null)
            {
                return NotFound();
            }

            var randomproducts = this.context.Products.Where(x => x.ProductID != id).OrderBy(x => Guid.NewGuid()).Take(6).ToList();


            ViewBag.randomproducts = randomproducts;


            return View(data);
        }

        public IActionResult Product()
        {

            ViewBag.products = this.context.Products.Include(x => x.categories).ToList(); 

            return View();
        }

    }
}
