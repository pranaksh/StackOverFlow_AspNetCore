using Microsoft.AspNetCore.Mvc;

namespace Stack.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
