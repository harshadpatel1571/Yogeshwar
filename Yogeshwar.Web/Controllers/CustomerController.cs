namespace Yogeshwar.Web.Controllers;

[Authorize]
public sealed class CustomerController : Controller
{
    private readonly Lazy<ICustomerService> _customerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerController"/> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    public CustomerController(Lazy<ICustomerService> customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Releases all resources currently used by this <see cref="T:Microsoft.AspNetCore.Mvc.Controller" /> instance.
    /// </summary>
    /// <param name="disposing"><c>true</c> if this method is being invoked by the <see cref="M:Microsoft.AspNetCore.Mvc.Controller.Dispose" /> method,
    /// otherwise <c>false</c>.</param>
    protected override void Dispose(bool disposing)
    {
        if (_customerService.IsValueCreated & disposing)
        {
            _customerService.Value.Dispose();
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

        var data = await _customerService.Value.GetByFilterAsync(filters).ConfigureAwait(false);

        var responseModel = new DataTableResponseDto<CustomerDto>
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
    /// <returns></returns>
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

    /// <summary>
    /// Add or edit.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddEdit(CustomerDto customer)
    {
        ModelState.Remove("Id");

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError();
            return View();
        }

        await _customerService.Value.CreateOrUpdateAsync(customer).ConfigureAwait(false);

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
            var count = await _customerService.Value.DeleteAsync(id).ConfigureAwait(false);

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
        var model = await _customerService.Value.GetSingleAsync(id).ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }
    
    /// <summary>
    /// Actives and in active record.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ActiveInActiveRecord(int id)
    {
        var result = await _customerService.Value.ActiveInActiveRecordAsync(id)
            .ConfigureAwait(false);

        if (result.Value is NotFound)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }
}