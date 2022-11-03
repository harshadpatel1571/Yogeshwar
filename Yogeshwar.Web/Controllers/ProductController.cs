namespace Yogeshwar.Web.Controllers;

[Authorize]
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

    [HttpPost]
    public async Task<IActionResult> BindQuantity(int id)
    {
        var data = await _productService.Value.GetAccessoriesQuantity(id).ConfigureAwait(false);
        return Ok(data);
    }

    public async ValueTask<IActionResult> AddEdit(int id, [FromServices] IDropDownService dropDownService)
    {
        using var _ = dropDownService;
        var dropDownData = await dropDownService.BindDropDownForAccessoriesAsync().ConfigureAwait(false);

        ProductDto model;

        if (id < 1)
        {
            model = new ProductDto
            {
                SelectListsForAccessories = new SelectList(dropDownData, "Key", "Text")
            };

            return View(model);
        }

        model = await _productService.Value.GetSingleAsync(id).ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        model.SelectListsForAccessories = new SelectList(dropDownData, "Key", "Text");

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddEdit(ProductDto productDto, [FromServices] IDropDownService dropDownService)
    {
        using var _ = dropDownService;
        ModelState.Remove("Id");

        productDto.AccessoriesQuantity ??= new List<AccessoriesQuantity>();

        for (int i = 0; i < Request.Form["AccessoriesQuantity.AccessoriesId"].Count; i++)
        {
            productDto.AccessoriesQuantity.Add(new AccessoriesQuantity
            {
                AccessoriesId = Convert.ToInt32(Request.Form["AccessoriesQuantity.AccessoriesId"][i]),
                Quantity = Convert.ToInt32(Request.Form["AccessoriesQuantity.Quantity"][i])
            });
        }

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError();

            var dropDownData = await dropDownService.BindDropDownForAccessoriesAsync().ConfigureAwait(false);
            productDto.SelectListsForAccessories = new SelectList(dropDownData, "Key", "Text");

            return View(productDto);
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

    public async Task<IActionResult> Detail(int id, [FromServices] IDropDownService dropDownService)
    {
        var model = await _productService.Value.GetSingleAsync(id).ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        var dropDownData = await dropDownService.BindDropDownForAccessoriesAsync().ConfigureAwait(false);
        model.SelectListsForAccessories = new SelectList(dropDownData, "Key", "Text");

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