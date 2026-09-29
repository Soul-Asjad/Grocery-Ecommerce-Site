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

        public IActionResult Pro()
        {
            return View();
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

            var randomproducts = this.context.Products.Where(x => x.ProductID != id).OrderBy(x => Guid.NewGuid()).Take(4).ToList();


            ViewBag.randomproducts = randomproducts;


            return View(data);
        }

        public IActionResult Product(int page = 1)
        {
            int page_size = 10;

            ViewBag.products = this.context.Products.Include(x => x.categories).Skip((page - 1) * page_size).Take(page_size).ToList();

            ViewBag.CurrentPage = page;

            int total_product = this.context.Products.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)total_product / page_size);

            return View();
        }


        //public IActionResult Product(string? search, int page = 1)
        //{
        //    int page_size = 10;

        //    var products = this.context.Products
        //        .Include(x => x.categories)
        //        .AsQueryable();

        //    // Search
        //    if (!string.IsNullOrWhiteSpace(search))
        //    {
        //        search = search.Trim();

        //        products = products.Where(x =>
        //            x.ProductName.Contains(search));
        //    }

        //    // Total products AFTER search
        //    int total_product = products.Count();

        //    // Total pages
        //    ViewBag.TotalPages =
        //        (int)Math.Ceiling((double)total_product / page_size);

        //    ViewBag.CurrentPage = page;

        //    // Products for current page
        //    ViewBag.products = products
        //        .Skip((page - 1) * page_size)
        //        .Take(page_size)
        //        .ToList();

        //    // Keep search value for the view
        //    ViewBag.Search = search;

        //    return View();
        //}




    }
}
