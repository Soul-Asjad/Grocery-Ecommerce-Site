using Grocery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public IActionResult Add_product()
        {
            if (HttpContext.Session.GetString("user_role") != "Admin")
            {
                return RedirectToAction("Login", "User");
            }

            var categories = this.context.Categories.ToList();
            ViewBag.catlist = new SelectList(categories , "Categoryid", "CategoryName");

            return View();
        }

        [HttpPost]
        public IActionResult Add_product(Products data, IFormFile Image)
        {
            if (HttpContext.Session.GetString("user_role") != "Admin")
            {
                return RedirectToAction("Login", "User");
            }

            if (Image != null)
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

        public async Task<IActionResult> View_Products()
        {
            if (HttpContext.Session.GetString("user_role") != "Admin")
            {
                return RedirectToAction("Login", "User");
            }

            var data = await context.Products.AsNoTracking().ToListAsync();

            return View(data);
        }

        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("user_role") != "Admin")
            {
                return RedirectToAction("Login", "User");
            }

            var data = this.context.Products.FirstOrDefault(x => x.ProductID == id);
            return View(data);
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("user_role") != "Admin")
            {
                return RedirectToAction("Login", "User");
            }

            var data = this.context.Products.FirstOrDefault(x => x.ProductID == id);
            this.context.Products.Remove(data);
            this.context.SaveChanges();
            return RedirectToAction("View_Products", "Products");
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("user_role") != "Admin")
            {
                return RedirectToAction("Login", "User");
            }

            var data = this.context.Products.FirstOrDefault(x => x.ProductID == id);
            var categories = this.context.Categories.ToList();
            ViewBag.catlist = new SelectList(categories, "Categoryid", "CategoryName");

            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Products data, IFormFile Image)
        {
            if (HttpContext.Session.GetString("user_role") != "Admin")
            {
                return RedirectToAction("Login", "User");
            }

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
