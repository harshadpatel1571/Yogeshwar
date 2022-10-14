namespace Yogeshwar.Web.Controllers;

public class ProductController : Controller
{
    private readonly Lazy<IProductService> _productService;

    public ProductController(Lazy<IProductService> productService)
    {
        _productService = productService;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing & _productService.IsValueCreated)
        {
            _productService.Value.Dispose();
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

        var data = await _productService.Value.GetByFilterAsync(filters).ConfigureAwait(false);

        var responseModel = new DataTableResponseDto<ProductDto>
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

        var model = await _productService.Value.GetSingleAsync(id).ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddEdit(ProductDto productDto)
    {
        ModelState.Remove("Id");

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError();
            return View();
        }

        await _productService.Value.CreateOrUpdateAsync(productDto).ConfigureAwait(false);

        return RedirectToActionPermanent(nameof(Index));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Delete(int id)
    {
        var dbModel = await _productService.Value.DeleteAsync(id).ConfigureAwait(false);

        if (dbModel is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    public async Task<IActionResult> Detail(int id)
    {
        var model = await _productService.Value.GetSingleAsync(id).ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var isDeleted = await _productService.Value.DeleteImageAsync(id)
            .ConfigureAwait(false);

        if (isDeleted)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteVideo(int id)
    {
        var isDeleted = await _productService.Value.DeleteVideoAsync(id)
            .ConfigureAwait(false);

        if (isDeleted)
        {
            return NoContent();
        }

        return NotFound();
    }
}