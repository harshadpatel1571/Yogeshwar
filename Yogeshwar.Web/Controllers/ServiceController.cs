namespace Yogeshwar.Web.Controllers;

/// <summary>
/// Class ServiceController. This class cannot be inherited.
/// Implements the <see cref="Controller" />
/// </summary>
/// <seealso cref="Controller" />
[Authorize]
public sealed class ServiceController : Controller
{
    /// <summary>
    /// The service service
    /// </summary>
    private readonly Lazy<IServiceService> _serviceService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceController" /> class.
    /// </summary>
    /// <param name="serviceService">The service service.</param>
    public ServiceController(Lazy<IServiceService> serviceService)
    {
        _serviceService = serviceService;
    }

    /// <summary>
    /// Releases all resources currently used by this <see cref="T:Microsoft.AspNetCore.Mvc.Controller" /> instance.
    /// </summary>
    /// <param name="disposing"><c>true</c> if this method is being invoked by the <see cref="M:Microsoft.AspNetCore.Mvc.Controller.Dispose" /> method,
    /// otherwise <c>false</c>.</param>
    protected override void Dispose(bool disposing)
    {
        if (_serviceService.IsValueCreated & disposing)
        {
            _serviceService.Value.Dispose();
        }

        base.Dispose(disposing);
    }

    /// <summary>
    /// Index view.
    /// </summary>
    /// <returns>IActionResult.</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Binds the data.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> BindData(CancellationToken cancellationToken)
    {
        var filters = DataExtractor.Extract(Request);

        var data = await _serviceService.Value.GetByFilterAsync(filters, cancellationToken).ConfigureAwait(false);

        var responseModel = new DataTableResponseDto<ServiceDto>
        {
            Draw = filters.Draw,
            Data = data.Data,
            RecordsFiltered = data.TotalCount,
            RecordsTotal = data.TotalCount
        };

        return Json(responseModel);
    }

    /// <summary>
    /// Add or edit.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="dropDownService">The drop down service.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    public async ValueTask<IActionResult> AddEdit(int id, [FromServices] IDropDownService dropDownService,
        CancellationToken cancellationToken)
    {
        using var _ = dropDownService;

        ViewBag.Orders = new SelectList(await dropDownService
                .BindDropDownForOrdersAsync(cancellationToken)
                .ConfigureAwait(false),
            "Key", "Text");

        ViewBag.Status = new SelectList(dropDownService.BindDropDownForService(), "Key", "Text");

        if (id < 1)
        {
            var view = View();
            return await Task.FromResult(view).ConfigureAwait(false);
        }

        var model = await _serviceService.Value.GetSingleAsync(id, cancellationToken).ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    /// <summary>
    /// Add or edit.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="dropDownService">The drop down service.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> AddEdit(ServiceDto service, [FromServices] IDropDownService dropDownService,
        CancellationToken cancellationToken)
    {
        ModelState.Remove("Id");

        using var _ = dropDownService;

        if (!ModelState.IsValid)
        {
            ViewBag.Orders = new SelectList(
                await dropDownService.BindDropDownForOrdersAsync(cancellationToken).ConfigureAwait(false),
                "Key", "Text");
            ViewBag.Status = new SelectList(dropDownService.BindDropDownForService(), "Key", "Text");

            ModelState.AddModelError();
            return View();
        }

        await _serviceService.Value.CreateOrUpdateAsync(service, cancellationToken).ConfigureAwait(false);

        return RedirectToActionPermanent(nameof(Index));
    }

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public async ValueTask<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            var count = await _serviceService.Value.DeleteAsync(id, cancellationToken).ConfigureAwait(false);

            if (count == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// Details the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    public async Task<IActionResult> Detail(int id, CancellationToken cancellationToken)
    {
        var model = await _serviceService.Value.GetSingleAsync(id, cancellationToken).ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }
}