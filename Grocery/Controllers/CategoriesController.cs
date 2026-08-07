using Grocery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Grocery.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly GroceryDBContext context;
        private readonly IWebHostEnvironment env;

        public CategoriesController(GroceryDBContext context,IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add_Category()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add_Category(Categories data, IFormFile Image)
        {
            if (Image != null)
            {
                var filename = Guid.NewGuid().ToString() + "_" + Image.FileName;
                var path = Path.Combine(env.WebRootPath, "All_Images/Categories/", filename);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }

                data.image = filename;
                this.context.Categories.Add(data);
                this.context.SaveChanges();
            }
            return RedirectToAction("View_category");
        }

        public IActionResult View_category()
        {
            var data = this.context.Categories.ToList();
            return View(data);
        }

        public IActionResult Delete(int id)
        {
            var data = this.context.Categories.FirstOrDefault(x => x.Categoryid == id);
            this.context.Categories.Remove(data);
            this.context.SaveChanges();
            return RedirectToAction("View_category");

        }

        public IActionResult Details(int id)
        {
            var data = this.context.Categories.FirstOrDefault(x => x.Categoryid == id);
            return View(data);

        }



        public IActionResult Edit(int id)
        {
            var data = this.context.Categories.FirstOrDefault(x => x.Categoryid == id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Categories data, IFormFile Image)
        {
            var old_data = this.context.Categories.FirstOrDefault(x => x.Categoryid == data.Categoryid);

            if (old_data == null)
            {
                return NotFound();
            }


            old_data.CategoryName = data.CategoryName;
            old_data.Description = data.Description;
            old_data.isactive = data.isactive;
            old_data.createdat = DateTime.UtcNow;

            if (Image != null)
            {
                var filename = Guid.NewGuid().ToString() + " _ " + Image.FileName;
                var path = Path.Combine(env.WebRootPath, "All_Images/Categories/", filename);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }

                old_data.image = filename;
            }

            this.context.SaveChanges();
            return RedirectToAction("View_category", "Categories");


        }



    }
}
