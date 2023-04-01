namespace Yogeshwar.Web.Controllers;

[Authorize]
public class ProductController : Controller
{
    private readonly Lazy<IProductService> _productService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductController"/> class.
    /// </summary>
    /// <param name="productService">The product service.</param>
    public ProductController(Lazy<IProductService> productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Releases all resources currently used by this <see cref="T:Microsoft.AspNetCore.Mvc.Controller" /> instance.
    /// </summary>
    /// <param name="disposing"><c>true</c> if this method is being invoked by the <see cref="M:Microsoft.AspNetCore.Mvc.Controller.Dispose" /> method,
    /// otherwise <c>false</c>.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing & _productService.IsValueCreated)
        {
            _productService.Value.Dispose();
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

    /// <summary>
    /// Binds the quantity.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> BindQuantity(int id)
    {
        var data = await _productService.Value.GetAccessoriesQuantity(id).ConfigureAwait(false);
        return Ok(data);
    }

    /// <summary>
    /// Add or edit.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="dropDownService">The drop down service.</param>
    /// <returns></returns>
    public async ValueTask<IActionResult> AddEdit(int id, [FromServices] IDropDownService dropDownService)
    {
        using var _ = dropDownService;
        var accessories = await dropDownService.BindDropDownForAccessoriesAsync().ConfigureAwait(false);
        var categories = await dropDownService.BindDropDownForCategoriesAsync().ConfigureAwait(false);

        ProductDto model;

        if (id < 1)
        {
            model = new ProductDto
            {
                SelectListsForAccessories = new SelectList(accessories, "Key", "Text"),
                SelectListsForCategories = new SelectList(categories, "Key", "Text")
            };

            return View(model);
        }

        model = await _productService.Value.GetSingleAsync(id).ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        model.SelectListsForAccessories = new SelectList(accessories, "Key", "Text");
        model.SelectListsForCategories = new SelectList(categories, "Key", "Text");

        return View(model);
    }

    /// <summary>
    /// Add or edit.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <param name="dropDownService">The drop down service.</param>
    /// <returns></returns>
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

            var accessories = await dropDownService.BindDropDownForAccessoriesAsync().ConfigureAwait(false);
            var categories = await dropDownService.BindDropDownForCategoriesAsync().ConfigureAwait(false);

            productDto.SelectListsForAccessories = new SelectList(accessories, "Key", "Text");
            productDto.SelectListsForCategories = new SelectList(categories, "Key", "Text");

            return View(productDto);
        }

        await _productService.Value.CreateOrUpdateAsync(productDto).ConfigureAwait(false);

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
            var count = await _productService.Value.DeleteAsync(id).ConfigureAwait(false);

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
    /// <param name="dropDownService">The drop down service.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Deletes the image.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Deletes the video.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Actives and in active record.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ActiveInActiveRecord(int id)
    {
        var result = await _productService.Value.ActiveInActiveRecordAsync(id)
            .ConfigureAwait(false);

        if (result.Value is NotFound)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }
}