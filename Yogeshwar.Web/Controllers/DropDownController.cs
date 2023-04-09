namespace Yogeshwar.Web.Controllers;

public class DropDownController : Controller
{
    private readonly IDropDownService _dropDownService;

    public DropDownController(IDropDownService dropDownService)
    {
        _dropDownService = dropDownService;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _dropDownService.Dispose();
    }

    public async Task<IActionResult> BindDropDownForAccessories(CancellationToken cancellationToken)
    {
        return Json(await _dropDownService.BindDropDownForAccessoriesAsync(cancellationToken));
    }
}
