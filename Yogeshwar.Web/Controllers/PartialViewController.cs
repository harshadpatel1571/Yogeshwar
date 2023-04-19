namespace Yogeshwar.Web.Controllers;

/// <summary>
/// Class PartialViewController.
/// Implements the <see cref="Controller" />
/// </summary>
/// <seealso cref="Controller" />
[Authorize]
public class PartialViewController : Controller
{
    /// <summary>
    /// Accessorieses the add popup view.
    /// </summary>
    /// <returns>IActionResult.</returns>
    public IActionResult AccessoriesAddPopupView()
    {
        return PartialView("~/Views/Accessories/_AccessoriesAddEdit.cshtml");
    }
    public IActionResult CategoryAddPopupView()
    {
        return PartialView("~/Views/Categories/_CategoryAddEdit.cshtml");
    }
}
