//using Grocery.Models;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace Grocery.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProductsApiController : Controller
//    {
//        private readonly GroceryDBContext context;
//        private readonly IWebHostEnvironment env;

//        public ProductsApiController(GroceryDBContext context , IWebHostEnvironment env)
//        {
//            this.context = context;
//            this.env = env;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpGet]
//        public IActionResult GetProduct()
//        {
//            var product = this.context.Products.Include(x => x.categories).ToList();

//            return Ok(product);
//        }

//        [HttpGet("{id}")]
//        public IActionResult GetProduct(int id)
//        {
//            var product = this.context.Products.Include(x => x.categories).FirstOrDefault(x => x.ProductID == id);

//            if(product == null)
//            {
//                return NotFound();
//            }

//            return Ok(product);
//        }

//        [HttpPost]
//        public IActionResult AddProduct(Products data,IFormFile Image)
//        {
//            if(Image != null)
//            {
//                var filename = Guid.NewGuid().ToString() + "-" + Image.FileName;
//                var path = Path.Combine(env.WebRootPath, "All_Images/Products", filename);

//                using(var stream = new FileStream(path,FileMode.Create))
//                {
//                    Image.CopyTo(stream);
//                }
//                data.Image = filename;
//            }


//            this.context.Products.Add(data);
//            this.context.SaveChanges();
//            return Ok(data);
//        }


//        [HttpPut("{id}")]
//        public IActionResult Edit(int id,Products data, IFormFile Image)
//        {
//            var old_product = this.context.Products.FirstOrDefault(x => x.ProductID == id);

//            if (old_product == null)
//            {
//                return NotFound();
//            }

//            old_product.ProductName = data.ProductName;
//            old_product.CategoryID = data.CategoryID;
//            old_product.Price = data.Price;
//            old_product.DiscountPrice = data.DiscountPrice;
//            old_product.Description = data.Description;
//            old_product.Unit = data.Unit;
//            old_product.StockQuantity = data.StockQuantity;
//            old_product.Isactive = data.Isactive;
//            old_product.Isfeatured = data.Isfeatured;
//            old_product.Createdat = DateTime.UtcNow;

//            if(Image != null)
//            {
//                var filename = Guid.NewGuid().ToString() + "-" + Image.FileName;
//                var path = Path.Combine(env.WebRootPath, "All_Images/Products", filename);

//                using (var stream = new FileStream(path, FileMode.Create))
//                {
//                    Image.CopyTo(stream);
//                }

//                old_product.Image = filename;
//            }

//            this.context.Products.Add(old_product);
//            this.context.SaveChanges();
//            return Ok(old_product);
//        }

//        [HttpDelete("{id}")]
//        public IActionResult Remove(int id)
//        {
//            var product = this.context.Products.FirstOrDefault(x => x.ProductID == id);
            
//            if(product == null)
//            {
//                return NotFound();
//            }

//            this.context.Products.Remove(product);
//            this.context.SaveChanges();
//            return Ok(product);
//        }

//    }
//}
