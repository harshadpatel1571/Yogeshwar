using Microsoft.VisualBasic;
using System.Threading;
using Yogeshwar.DB.DbModels;

namespace Yogeshwar.Web.Controllers;

[Authorize]
public class ConfigurationController : Controller
{
    private readonly Lazy<IConfigurationService> _configurationService;

    public ConfigurationController(Lazy<IConfigurationService> configurationService)
    {
        _configurationService = configurationService;
    }

    protected override void Dispose(bool disposing)
    {
        if (_configurationService.IsValueCreated & disposing)
        {
            _configurationService.Value.Dispose();
        }

        base.Dispose(disposing);
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await _configurationService.Value
            .GetSingleAsync(cancellationToken)
            .ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    public async Task<IActionResult> AddEdit(ConfigurationDto configurationDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError();
            return View();
        }

        await _configurationService.Value.UpdateAsync(configurationDto, cancellationToken).ConfigureAwait(false);

        return RedirectToActionPermanent(nameof(Index), new { msg = "success" });
    }
}
