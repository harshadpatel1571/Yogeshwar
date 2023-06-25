namespace Yogeshwar.Web.Controllers;

/// <summary>
/// Class CategoriesController.
/// Implements the <see cref="Controller" />
/// </summary>
/// <seealso cref="Controller" />
public class CategoriesController : Controller
{
    /// <summary>
    /// The category service
    /// </summary>
    private readonly Lazy<ICategoryService> _categoryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoriesController" /> class.
    /// </summary>
    /// <param name="categoryService">The category service.</param>
    public CategoriesController(Lazy<ICategoryService> categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Releases all resources currently used by this <see cref="T:Microsoft.AspNetCore.Mvc.Controller" /> instance.
    /// </summary>
    /// <param name="disposing"><c>true</c> if this method is being invoked by the <see cref="M:Microsoft.AspNetCore.Mvc.Controller.Dispose" /> method,
    /// otherwise <c>false</c>.</param>
    protected override void Dispose(bool disposing)
    {
        if (_categoryService.IsValueCreated & disposing)
        {
            _categoryService.Value.Dispose();
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

        var data = await _categoryService.Value.GetByFilterAsync(filters, cancellationToken)
            .ConfigureAwait(false);

        var responseModel = new DataTableResponseDto<CategoryDto>
        {
            Draw = filters.Draw,
            Data = data.Data,
            RecordsFiltered = data.TotalCount,
            RecordsTotal = data.TotalCount
        };

        return Json(responseModel);
    }

    /// <summary>
    /// Adds edit.
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

        var model = await _categoryService.Value.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    /// <summary>
    /// Adds edit.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> AddEdit(CategoryDto category, CancellationToken cancellationToken)
    {
        ModelState.Remove("Id");

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError();
            return View();
        }

        await _categoryService.Value.CreateOrUpdateAsync(category, cancellationToken).ConfigureAwait(false);

        return RedirectToActionPermanent(nameof(Index), new { msg = "success" });
    }

    [HttpPost]
    public async Task<IActionResult> AddEditPopup(CategoryDto categoryDto, CancellationToken cancellationToken)
    {
        ModelState.Remove("Id");

        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetKeyErrors();
            return BadRequest(errors);
        }

        await _categoryService.Value.CreateOrUpdateAsync(categoryDto, cancellationToken).ConfigureAwait(false);

        return Ok(categoryDto);
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
            var dbModel = await _categoryService.Value
                .DeleteAsync(id, cancellationToken).ConfigureAwait(false);

            if (dbModel is null)
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
        var model = await _categoryService.Value
            .DeleteImageAsync(id, cancellationToken)
            .ConfigureAwait(false);

        if (model is not null)
        {
            return NoContent();
        }

        return NotFound();
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
        var model = await _categoryService.Value
            .ActiveInActiveRecordAsync(id, cancellationToken)
            .ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        return Ok(model.IsActive);
    }
}