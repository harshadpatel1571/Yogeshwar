namespace Yogeshwar.Web.Controllers;

[Authorize]
public sealed class ServiceController : Controller
{
    private readonly Lazy<IServiceService> _serviceService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceController"/> class.
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
    /// <returns></returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Binds the data.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> BindData()
    {
        var filters = DataExtractor.Extract(Request);

        var data = await _serviceService.Value.GetByFilterAsync(filters).ConfigureAwait(false);

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
    /// <returns></returns>
    public async ValueTask<IActionResult> AddEdit(int id, [FromServices] IDropDownService dropDownService)
    {
        using var _ = dropDownService;

        ViewBag.Orders = new SelectList(await dropDownService.BindDropDownForOrdersAsync().ConfigureAwait(false), "Key",
            "Text");
        ViewBag.Status = new SelectList(dropDownService.BindDropDownForService(), "Key", "Text");

        if (id < 1)
        {
            var view = View();
            return await Task.FromResult(view).ConfigureAwait(false);
        }

        var model = await _serviceService.Value.GetSingleAsync(id).ConfigureAwait(false);

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
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddEdit(ServiceDto service, [FromServices] IDropDownService dropDownService)
    {
        ModelState.Remove("Id");

        using var _ = dropDownService;

        if (!ModelState.IsValid)
        {
            ViewBag.Orders = new SelectList(await dropDownService.BindDropDownForOrdersAsync().ConfigureAwait(false),
                "Key", "Text");
            ViewBag.Status = new SelectList(dropDownService.BindDropDownForService(), "Key", "Text");

            ModelState.AddModelError();
            return View();
        }

        await _serviceService.Value.CreateOrUpdateAsync(service).ConfigureAwait(false);

        return RedirectToActionPermanent(nameof(Index));
    }

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpPost]
    public async ValueTask<IActionResult> Delete(int id)
    {
        try
        {
            var count = await _serviceService.Value.DeleteAsync(id).ConfigureAwait(false);

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
    /// <returns></returns>
    public async Task<IActionResult> Detail(int id)
    {
        var model = await _serviceService.Value.GetSingleAsync(id).ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }
}