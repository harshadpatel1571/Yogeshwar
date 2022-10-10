using Yogeshwar.Helper.Extension;

namespace Yogeshwar.Web.Controllers;

[Authorize]
public sealed class CustomerController : Controller
{
    private readonly Lazy<ICustomerService> _customerService;

    public CustomerController(Lazy<ICustomerService> customerService)
    {
        _customerService = customerService;
    }

    protected override void Dispose(bool disposing)
    {
        if (_customerService.IsValueCreated)
        {
            _customerService.Value.Dispose();
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

        var data = await _customerService.Value.GetByFilterAsync(filters).ConfigureAwait(false);

        var responseModel = new DataTableResponseDto<CustomerDto>
        {
            Draw = filters.Draw,
            Data = data,
            RecordsFiltered = data.Count,
            RecordsTotal = data.Count,
        };

        return Json(responseModel);
    }

    public async ValueTask<IActionResult> AddEdit(int id)
    {
        if (id < 1)
        {
            var view = View();
            return await Task.FromResult(view).ConfigureAwait(false);
        }

        var model = await _customerService.Value.GetSingleAsync(id).ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddEdit(CustomerDto customer)
    {
        ModelState.Remove("Id");

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("","");
            return View();
        }

        await _customerService.Value.CreateOrUpdateAsync(customer).ConfigureAwait(false);

        return RedirectToActionPermanent(nameof(Index));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Delete(int id)
    {
        var dbModel = await _customerService.Value.DeleteAsync(id).ConfigureAwait(false);

        if (dbModel is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    public async Task<IActionResult> Detail(CustomerDto customer)
    {
        return View();
    }
}