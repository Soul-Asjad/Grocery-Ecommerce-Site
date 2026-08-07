using Grocery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Grocery.Controllers
{
    public class ProductsController : Controller
    {

        private readonly GroceryDBContext context;
        private readonly IWebHostEnvironment env;

        public ProductsController(GroceryDBContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add_product()
        {
            var categories = this.context.Categories.ToList();
            ViewBag.catlist = new SelectList(categories , "Categoryid", "CategoryName");

            return View();
        }

        [HttpPost]
        public IActionResult Add_product(Products data, IFormFile Image)
        {
             if(Image != null)
                {
                    var filename = Guid.NewGuid().ToString() + " _ " + Image.FileName;
                    var path = Path.Combine(env.WebRootPath, "All_Images/Products/", filename);

                    using(var stream = new FileStream(path, FileMode.Create))
                    {
                        Image.CopyTo(stream);
                    }

                    data.Image = filename;
                    this.context.Products.Add(data);
                    this.context.SaveChanges();
                }
                return RedirectToAction("View_Products");

        }

        public IActionResult View_Products()
        {
            var data = this.context.Products.ToList();
            return View(data);
        }

        public IActionResult Details(int id)
        {
            var data = this.context.Products.FirstOrDefault(x => x.ProductID == id);
            return View(data);
        }

        public IActionResult Delete(int id)
        {
            var data = this.context.Products.FirstOrDefault(x => x.ProductID == id);
            this.context.Products.Remove(data);
            this.context.SaveChanges();
            return RedirectToAction("View_Products", "Products");
        }

        public IActionResult Edit(int id)
        {
            var data = this.context.Products.FirstOrDefault(x => x.ProductID == id);
            var categories = this.context.Categories.ToList();
            ViewBag.catlist = new SelectList(categories, "Categoryid", "CategoryName");

            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Products data, IFormFile Image)
        {
            var old_data = this.context.Products.FirstOrDefault(x => x.ProductID == data.ProductID);

            if(old_data == null)
            {
                return NotFound();
            }


            old_data.ProductName = data.ProductName;
            old_data.Price = data.Price;
            old_data.Description = data.Description;
            old_data.CategoryID = data.CategoryID;
            old_data.DiscountPrice = data.DiscountPrice;
            old_data.StockQuantity = data.StockQuantity;
            old_data.Unit = data.Unit;
            old_data.Isfeatured = data.Isfeatured;
            old_data.Isactive = data.Isactive;
            old_data.Createdat = DateTime.UtcNow;
            
            if(Image != null)
            {
                var filename = Guid.NewGuid().ToString() + " _ " + Image.FileName;
                var path = Path.Combine(env.WebRootPath, "All_Images/Products/", filename);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }

                old_data.Image = filename;
            }

            this.context.SaveChanges();
            return RedirectToAction("View_Products", "Products");


        }


    }
}
