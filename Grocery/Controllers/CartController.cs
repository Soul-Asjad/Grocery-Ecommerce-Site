using Grocery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grocery.Controllers
{
    public class CartController : Controller
    {

        private readonly GroceryDBContext context;

        public CartController(GroceryDBContext context)
        {
            this.context = context;
        }


        [HttpPost]
        public IActionResult AddtoCart(int productid, int quantity)
        {
            var session_id = HttpContext.Session.GetInt32("user_id");                        //  Session Id Checking

            if (session_id == null)
            {
                return RedirectToAction("Login", "User");
            }

            var checking_cart = this.context.Cart.FirstOrDefault(x => x.UserID == session_id);        //  Cart CHecking and add in database

            if (checking_cart == null)
            {
                checking_cart = new Cart
                {
                    UserID = session_id.Value
                };

                this.context.Cart.Add(checking_cart);
                this.context.SaveChanges();
            }


            //   Existing product checking and adding in database


            var existing_product = this.context.CartItems.FirstOrDefault(x => x.ProductID == productid && x.CartID == checking_cart.CartID);
            var product = this.context.Products.FirstOrDefault(x => x.ProductID == productid);

            if (quantity < 1)
            {
                quantity = 1;
            }

            if (existing_product != null)
            {
                existing_product.Quantity += quantity;
            }
            else
            {
                existing_product = new CartItems
                {
                    CartID = checking_cart.CartID,
                    ProductID = productid,
                    Quantity = quantity,
                    price = product.Price
                };

                this.context.CartItems.Add(existing_product);
            }

            this.context.SaveChanges();
            return RedirectToAction("Product", "Home");
        }


        public IActionResult Shopping_Cart()
        {
            var session_id = HttpContext.Session.GetInt32("user_id");

            if (session_id == null)
            {
                return RedirectToAction("Login", "User");
            }

            var cart = this.context.Cart.FirstOrDefault(x => x.UserID == session_id);

            if (cart == null)
            {
                return View(new List<CartItems>());
            }

            var cartitems = this.context.CartItems.Include(x => x.products)
                .ThenInclude(x => x.categories).Where(x => x.CartID == cart.CartID).ToList();

            HttpContext.Session.SetInt32("cartcount", cartitems.Count);
            ViewBag.count = cartitems.Count;

            return View(cartitems);
        }

        public IActionResult Remove(int id)
        {
            var item = this.context.CartItems.FirstOrDefault(x => x.CartItemID == id);

            this.context.CartItems.Remove(item);
            this.context.SaveChanges();
            return RedirectToAction("Shopping_Cart");
        }


        // Updated: returns JSON instead of redirecting, so the page doesn't refresh
        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var cartitem = this.context.CartItems.Include(x => x.products)
                .FirstOrDefault(x => x.CartItemID == id);

            if (cartitem == null)
            {
                return NotFound();
            }

            if (quantity < 1)
            {
                quantity = 1;
            }

            cartitem.Quantity = quantity;
            this.context.SaveChanges();

            var cartItems = this.context.CartItems.Include(x => x.products)
                .Where(x => x.CartID == cartitem.CartID).ToList();

            decimal subtotal = cartItems.Sum(x => x.products.Price * x.Quantity);
            decimal delivery = 2.00m;

            return Json(new
            {
                quantity = cartitem.Quantity,
                itemTotal = cartitem.products.Price * cartitem.Quantity,
                subtotal = subtotal,
                total = subtotal + delivery
            });
        }

    }
}