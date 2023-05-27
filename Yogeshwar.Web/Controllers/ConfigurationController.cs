namespace Yogeshwar.Web.Controllers;

/// <summary>
/// Class ConfigurationController.
/// Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller" />
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
[Authorize]
public class ConfigurationController : Controller
{
    /// <summary>
    /// The configuration service
    /// </summary>
    private readonly Lazy<IConfigurationService> _configurationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationController" /> class.
    /// </summary>
    /// <param name="configurationService">The configuration service.</param>
    public ConfigurationController(Lazy<IConfigurationService> configurationService)
    {
        _configurationService = configurationService;
    }

    /// <summary>
    /// Releases all resources currently used by this <see cref="T:Microsoft.AspNetCore.Mvc.Controller" /> instance.
    /// </summary>
    /// <param name="disposing"><c>true</c> if this method is being invoked by the <see cref="M:Microsoft.AspNetCore.Mvc.Controller.Dispose" /> method,
    /// otherwise <c>false</c>.</param>
    protected override void Dispose(bool disposing)
    {
        if (_configurationService.IsValueCreated & disposing)
        {
            _configurationService.Value.Dispose();
        }

        base.Dispose(disposing);
    }

    /// <summary>
    /// Indexes the specified cancellation token.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await _configurationService.Value
            .GetSingleAsync(cancellationToken)
            .ConfigureAwait(false);

        return View(model);
    }

    /// <summary>
    /// Adds the edit.
    /// </summary>
    /// <param name="configurationDto">The configuration dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    public async Task<IActionResult> AddEdit(ConfigurationDto configurationDto, CancellationToken cancellationToken)
    {
        ModelState.Remove("Id");
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError();
            return RedirectToActionPermanent(nameof(Index));
        }

        await _configurationService.Value.UpdateAsync(configurationDto, cancellationToken).ConfigureAwait(false);

        return RedirectToActionPermanent(nameof(Index), new { msg = "success" });
    }
}
