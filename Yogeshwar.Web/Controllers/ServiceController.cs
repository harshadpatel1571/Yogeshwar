using Microsoft.AspNetCore.Mvc;

namespace Yogeshwar.Web.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async ValueTask<IActionResult> AddEdit([FromServices] IDropDownService dropDownService)
        {
            using var _ = dropDownService;

            ViewBag.Customers = new SelectList(await dropDownService.BindDropDownForCustomersAsync(),
                "Key", "Text");
            ViewBag.Status = new SelectList(dropDownService.BindDropDownForStatus(),
                "Key", "Text");

            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}
