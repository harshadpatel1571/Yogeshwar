namespace Yogeshwar.Web.Controllers;

/// <summary>
/// Class HomeController. This class cannot be inherited.
/// Implements the <see cref="Controller" />
/// </summary>
/// <seealso cref="Controller" />
[Authorize]
public sealed class HomeController : Controller
{
    /// <summary>
    /// Index view.
    /// </summary>
    /// <returns>IActionResult.</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Handle the status code.
    /// </summary>
    /// <param name="statusCode">The status code.</param>
    /// <returns>IActionResult.</returns>
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

    /// <summary>
    /// Handles error.
    /// </summary>
    /// <returns>IActionResult.</returns>
    public IActionResult Error()
    {
        //var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        return View("500");
    }
}