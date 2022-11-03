namespace Yogeshwar.Web.Controllers;

[Authorize]
public sealed class ServiceController : Controller
{
    private readonly Lazy<IServiceService> _serviceService;

    public ServiceController(Lazy<IServiceService> serviceService)
    {
        _serviceService = serviceService;
    }

    protected override void Dispose(bool disposing)
    {
        if (_serviceService.IsValueCreated & disposing)
        {
            _serviceService.Value.Dispose();
        }

        base.Dispose(disposing);
    }

    public IActionResult Index()
    {
        return View();
    }

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

    public async ValueTask<IActionResult> AddEdit([FromServices] IDropDownService dropDownService, int id)
    {
        using var _ = dropDownService;

        ViewBag.Customers = new SelectList(await dropDownService.BindDropDownForCustomersAsync().ConfigureAwait(false),
            "Key", "Text");
        ViewBag.Status = new SelectList(dropDownService.BindDropDownForService(),
            "Key", "Text");

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

    [HttpPost]
    public async Task<IActionResult> AddEdit(ServiceDto service)
    {
        ModelState.Remove("Id");

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError();
            return View();
        }

        await _serviceService.Value.CreateOrUpdateAsync(service).ConfigureAwait(false);

        return RedirectToActionPermanent(nameof(Index));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Delete(int id)
    {
        var dbModel = await _serviceService.Value.DeleteAsync(id).ConfigureAwait(false);

        if (dbModel is null)
        {
            return NotFound();
        }

        return NoContent();
    }

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
