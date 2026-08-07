using Grocery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Grocery.Controllers
{
    public class UserController : Controller
    {
        private readonly GroceryDBContext context;

        public UserController(GroceryDBContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(Users data)
        {
            if (ModelState.IsValid)
            {

                var user = this.context.User.FirstOrDefault(x => x.email == data.email);

                if (user != null)
                {
                    TempData["error"] = "Email is already exist!!!";
                    return View();
                }

                data.Password = BCrypt.Net.BCrypt.HashPassword(data.Password);
                this.context.User.Add(data);
                this.context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User_login user)
        {
            var verified_user = this.context.User.Where(x => x.email == user.email).FirstOrDefault();

            if (verified_user == null || !BCrypt.Net.BCrypt.Verify(user.Password, verified_user.Password))
            {
                TempData["error"] = "Invalid credentials";
            }


            TempData["success"] = "Login Succdesfully!!";
            HttpContext.Session.SetString("user", verified_user.FullName);
            HttpContext.Session.SetInt32("user_id", verified_user.Userid);
            HttpContext.Session.SetString("user_role", verified_user.role);
            return RedirectToAction("Index", "Home");

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("admin");
            HttpContext.Session.Remove("admin_id");

            HttpContext.Session.Remove("user");
            HttpContext.Session.Remove("user_id");

            return RedirectToAction("Index", "Home");
        }

    }
}
