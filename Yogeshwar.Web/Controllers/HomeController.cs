namespace Yogeshwar.Web.Controllers;

[Authorize]
public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
