namespace Yogeshwar.Web.Controllers;

/// <summary>
/// Class CustomerController. This class cannot be inherited.
/// Implements the <see cref="Controller" />
/// </summary>
/// <seealso cref="Controller" />
[Authorize]
public sealed class CustomerController : Controller
{
    /// <summary>
    /// The customer service
    /// </summary>
    private readonly Lazy<ICustomerService> _customerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerController" /> class.
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

        var data = await _customerService.Value.GetByFilterAsync(filters, cancellationToken)
            .ConfigureAwait(false);

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
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    public async ValueTask<IActionResult> AddEdit(int id, CancellationToken cancellationToken)
    {
        if (id < 1)
        {
            var view = View();
            return await Task.FromResult(view).ConfigureAwait(false);
        }

        var model = await _customerService.Value
            .GetSingleAsync(id, cancellationToken)
            .ConfigureAwait(false);

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
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> AddEdit(CustomerDto customer, CancellationToken cancellationToken)
    {
        ModelState.Remove("Id");

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError();
            return View(customer);
        }

        await _customerService.Value
            .UpsertAsync(customer, cancellationToken)
            .ConfigureAwait(false);

        return RedirectToActionPermanent(nameof(Index), new { msg = "success" });
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
            var count = await _customerService.Value
                .DeleteAsync(id, cancellationToken)
                .ConfigureAwait(false);

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
        var model = await _customerService.Value
            .GetSingleAsync(id, cancellationToken)
            .ConfigureAwait(false);

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
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> ActiveInActiveRecord(int id, CancellationToken cancellationToken)
    {
        var result = await _customerService.Value
            .ActiveInActiveRecordAsync(id, cancellationToken)
            .ConfigureAwait(false);

        if (result.Value is NotFound)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Deletes the image.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> DeleteImage(int id, CancellationToken cancellationToken)
    {
        var isDeleted = await _customerService.Value
            .DeleteImageAsync(id, cancellationToken)
            .ConfigureAwait(false);

        if (isDeleted)
        {
            return NoContent();
        }

        return NotFound();
    }
}