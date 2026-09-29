using Grocery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grocery.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly GroceryDBContext context;

        public CheckoutController(GroceryDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
             var session_id = HttpContext.Session.GetInt32("user_id");

            if(session_id == null)
            {
                return RedirectToAction("Login", "User");
            }

            var cart = this.context.Cart.FirstOrDefault(x => x.UserID == session_id);

            if(cart == null)
            {
                return RedirectToAction("Shopping_Cart", "Cart");
            }

            var items = this.context.CartItems.Include(x => x.products).ThenInclude(x => x.categories)
                .Where(x => x.CartID == cart.CartID).ToList();


            if (!items.Any())
            {
                return RedirectToAction("Shopping_Cart", "Cart");
            }

            ViewBag.cartitem = items;

            return View();
        }



        [HttpPost]
        public IActionResult Checkout(Addresses address, Payments payment)
        {

            var session = HttpContext.Session.GetInt32("user_id");

            if (session == null)
            {
                return RedirectToAction("Login", "Cart");
            }

            var cart = this.context.Cart.FirstOrDefault(x => x.UserID == session);

            if (cart == null)
            {
                return RedirectToAction("Shopping_Cart", "Cart");
            }

            var items = this.context.CartItems.Include(x => x.products).Where(x => x.CartID == cart.CartID).ToList();

            if (!items.Any())
            {
                return RedirectToAction("Shopping_Cart", "Cart");
            }

            address.UserID = session.Value;

            if (string.IsNullOrWhiteSpace(address.Address))
            {
                return Content("Please enter your address.");
            }

            this.context.Addresss.Add(address);
            this.context.SaveChanges();

            //Order section

            Orders order = new Orders();

            order.UserID = session.Value;
            order.AddressID = address.AddressID;
            order.OrderNumber = "ORD-" + Guid.NewGuid().ToString("N")[..8].ToUpper();
            order.PaymentStatus = "Pending";
            order.TotalAmount = items.Sum(x => x.products.Price * x.Quantity);
            order.PaymentStatus = "Pending";
            order.Orderstatus = "Pending";

            this.context.Orders.Add(order);
            this.context.SaveChanges();

            // payment section

            payment.OrderID = order.OrderID;
            payment.PaymentStatus = "Pending";
            payment.TransactionID = "TXN-" + Guid.NewGuid().ToString("N")[..8].ToUpper();
            payment.PaidAmount = order.TotalAmount;

            this.context.Payments.Add(payment);
            this.context.SaveChanges();


            // Order items 

            foreach (var item in items)
            {
                OrderItems orderitem = new OrderItems();

                orderitem.OrderID = order.OrderID;
                orderitem.ProductID = item.ProductID;
                orderitem.Quantity = item.Quantity;
                orderitem.Price = item.products.Price;
                orderitem.ProductName = item.products.ProductName;
                orderitem.TotalPrice = item.Quantity * item.products.Price;

                this.context.OrderItems.Add(orderitem);
            }
            this.context.SaveChanges();

            this.context.RemoveRange(items);
            this.context.SaveChanges();

            return RedirectToAction("OrderSuccess","Checkout");


        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

    }
}
