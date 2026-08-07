using Microsoft.AspNetCore.Mvc;

namespace Grocery.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Categories()
        {
            return View();
        }



    }
}
