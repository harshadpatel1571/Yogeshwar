using Microsoft.AspNetCore.Mvc;

namespace Yogeshwar.Web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
