namespace Yogeshwar.Web.Controllers;

public class CategoriesController : Controller
{
    private readonly Lazy<ICategoryService> _categoryService;

    public CategoriesController(Lazy<ICategoryService> categoryService)
    {
        _categoryService = categoryService;
    }

    protected override void Dispose(bool disposing)
    {
        if (_categoryService.IsValueCreated & disposing)
        {
            _categoryService.Value.Dispose();
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

        var data = await _categoryService.Value.GetByFilterAsync(filters).ConfigureAwait(false);

        var responseModel = new DataTableResponseDto<CategoryDto>
        {
            Draw = filters.Draw,
            Data = data.Data,
            RecordsFiltered = data.TotalCount,
            RecordsTotal = data.TotalCount
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

        var model = await _categoryService.Value.GetSingleAsync(id).ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddEdit(CategoryDto category)
    {
        ModelState.Remove("Id");

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError();
            return View();
        }

        await _categoryService.Value.CreateOrUpdateAsync(category).ConfigureAwait(false);

        return RedirectToActionPermanent(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var dbModel = await _categoryService.Value.DeleteAsync(id).ConfigureAwait(false);

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
}
