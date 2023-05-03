namespace Yogeshwar.Web.Controllers;

/// <summary>
/// Class AccessoriesController.
/// Implements the <see cref="Controller" />
/// </summary>
/// <seealso cref="Controller" />
[Authorize]
public class AccessoriesController : Controller
{
    /// <summary>
    /// The accessories service
    /// </summary>
    private readonly Lazy<IAccessoriesService> _accessoriesService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessoriesController" /> class.
    /// </summary>
    /// <param name="accessoriesService">The accessories service.</param>
    public AccessoriesController(Lazy<IAccessoriesService> accessoriesService)
    {
        _accessoriesService = accessoriesService;
    }

    /// <summary>
    /// Releases all resources currently used by this <see cref="T:Microsoft.AspNetCore.Mvc.Controller" /> instance.
    /// </summary>
    /// <param name="disposing"><c>true</c> if this method is being invoked by the <see cref="M:Microsoft.AspNetCore.Mvc.Controller.Dispose" /> method,
    /// otherwise <c>false</c>.</param>
    protected override void Dispose(bool disposing)
    {
        if (_accessoriesService.IsValueCreated & disposing)
        {
            _accessoriesService.Value.Dispose();
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

        var data = await _accessoriesService.Value
            .GetByFilterAsync(filters, cancellationToken)
            .ConfigureAwait(false);

        var responseModel = new DataTableResponseDto<AccessoriesDto>
        {
            Draw = filters.Draw,
            Data = data.Data,
            RecordsFiltered = data.TotalCount,
            RecordsTotal = data.TotalCount
        };

        return Json(responseModel);
    }

    /// <summary>
    /// Adds or edit.
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

        var model = await _accessoriesService.Value
            .GetSingleAsync(id, cancellationToken)
            .ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    /// <summary>
    /// Adds or edit.
    /// </summary>
    /// <param name="accessory">The accessory.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> AddEdit(AccessoriesDto accessory, CancellationToken cancellationToken)
    {
        ModelState.Remove("Id");

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError();
            return View();
        }

        await _accessoriesService.Value.CreateOrUpdateAsync(accessory, cancellationToken).ConfigureAwait(false);

        return RedirectToActionPermanent(nameof(Index), new { msg = "success" });
    }

    /// <summary>
    /// Adds the edit popup.
    /// </summary>
    /// <param name="accessory">The accessory.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> AddEditPopup(AccessoriesDto accessory, CancellationToken cancellationToken)
    {
        ModelState.Remove("Id");

        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetKeyErrors();
            return BadRequest(errors);
        }

        await _accessoriesService.Value.CreateOrUpdateAsync(accessory, cancellationToken).ConfigureAwait(false);

        return Ok(accessory);
    }

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            var count = await _accessoriesService.Value
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
    /// Deletes the image.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> DeleteImage(int id, CancellationToken cancellationToken)
    {
        var isDeleted = await _accessoriesService.Value
            .DeleteImageAsync(id, cancellationToken)
            .ConfigureAwait(false);

        if (isDeleted)
        {
            return NoContent();
        }

        return NotFound();
    }

    /// <summary>
    /// Details the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    public async Task<IActionResult> Detail(int id, CancellationToken cancellationToken)
    {
        var model = await _accessoriesService.Value
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
        var result = await _accessoriesService.Value
            .ActiveInActiveRecordAsync(id, cancellationToken)
            .ConfigureAwait(false);

        if (result.Value is NotFound)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }
}