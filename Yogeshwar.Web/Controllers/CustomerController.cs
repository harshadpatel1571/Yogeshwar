using Microsoft.AspNetCore.Mvc;

namespace Yogeshwar.Web.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddEdit()
        {
            return View();
        }
    }
}
