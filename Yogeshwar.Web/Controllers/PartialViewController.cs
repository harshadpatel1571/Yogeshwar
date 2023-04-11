namespace Yogeshwar.Web.Controllers;

/// <summary>
/// Class PartialViewController.
/// Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller" />
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
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
}
