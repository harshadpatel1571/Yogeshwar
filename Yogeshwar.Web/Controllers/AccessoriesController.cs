namespace Yogeshwar.Web.Controllers;

[Authorize]
public class AccessoriesController : Controller
{
    private readonly Lazy<IAccessoriesService> _accessoriesService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessoriesController"/> class.
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

        var data = await _accessoriesService.Value.GetByFilterAsync(filters).ConfigureAwait(false);

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
    /// <returns></returns>
    public async ValueTask<IActionResult> AddEdit(int id)
    {
        if (id < 1)
        {
            var view = View();
            return await Task.FromResult(view).ConfigureAwait(false);
        }

        var model = await _accessoriesService.Value.GetSingleAsync(id).ConfigureAwait(false);

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
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddEdit(AccessoriesDto accessory)
    {
        ModelState.Remove("Id");

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError();
            return View();
        }

        await _accessoriesService.Value.CreateOrUpdateAsync(accessory).ConfigureAwait(false);

        return RedirectToActionPermanent(nameof(Index));
    }

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var count = await _accessoriesService.Value.DeleteAsync(id).ConfigureAwait(false);

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
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var isDeleted = await _accessoriesService.Value.DeleteImageAsync(id)
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
    /// <returns></returns>
    public async Task<IActionResult> Detail(int id)
    {
        var model = await _accessoriesService.Value.GetSingleAsync(id).ConfigureAwait(false);

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
        var result = await _accessoriesService.Value.ActiveInActiveRecordAsync(id)
            .ConfigureAwait(false);

        if (result.Value is NotFound)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }
}