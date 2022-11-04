namespace Yogeshwar.Web.Controllers;

[Authorize]
public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("/Error/{statusCode:int}")]
    public IActionResult StatusCodeHandle(int statusCode)
    {
        var view = statusCode switch
        {
            404 => (IActionResult)View("404"),
            _ => StatusCode(statusCode)
        };

        return view;
    }

    public IActionResult Error()
    {
        //var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        return View("500");
    }
}