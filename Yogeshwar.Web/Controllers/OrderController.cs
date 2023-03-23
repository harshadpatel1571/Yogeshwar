namespace Yogeshwar.Web.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly Lazy<IOrderService> _orderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderController"/> class.
    /// </summary>
    /// <param name="orderService">The order service.</param>
    public OrderController(Lazy<IOrderService> orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Releases all resources currently used by this <see cref="T:Microsoft.AspNetCore.Mvc.Controller" /> instance.
    /// </summary>
    /// <param name="disposing"><c>true</c> if this method is being invoked by the <see cref="M:Microsoft.AspNetCore.Mvc.Controller.Dispose" /> method,
    /// otherwise <c>false</c>.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing & _orderService.IsValueCreated)
        {
            _orderService.Value.Dispose();
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

        var data = await _orderService.Value.GetByFilterAsync(filters).ConfigureAwait(false);

        var responseModel = new DataTableResponseDto<OrderDto>
        {
            Draw = filters.Draw,
            Data = data.Data,
            RecordsFiltered = data.TotalCount,
            RecordsTotal = data.TotalCount
        };

        return Json(responseModel);
    }

    /// <summary>
    /// Gets the accessories detail.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> GetAccessoriesDetail([FromQuery] int productId)
    {
        var detail = await _orderService.Value.GetAccessoriesAsync(productId).ConfigureAwait(false);

        if (detail is null)
        {
            return NotFound();
        }

        return Json(detail);
    }
    


    /// <summary>
    /// Add or edit.
    /// </summary>
    /// <param name="dropDownService">The drop down service.</param>
    /// <returns></returns>
    public async Task<IActionResult> AddEdit([FromServices] IDropDownService dropDownService)
    {
        using var _ = dropDownService;

        ViewBag.Customers = new SelectList(await dropDownService.BindDropDownForCustomersAsync().ConfigureAwait(false),
            "Key", "Text");
        ViewBag.Status = new SelectList(dropDownService.BindDropDownForOrderStatus(),
            "Key", "Text");
        ViewBag.OrderStatus = new SelectList(dropDownService.BindDropDownForOrderDetailStatus(),
            "Key", "Text");
        ViewBag.Products = new SelectList(await dropDownService.BindDropDownForProductsAsync().ConfigureAwait(false),
            "Key", "Text");

        return View();
    }

    /// <summary>
    /// Add or edit.
    /// </summary>
    /// <param name="orderDto">The order dto.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddEdit(OrderDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetKeyErrors();
            return BadRequest(errors);
        }

        await _orderService.Value.CreateOrUpdateAsync(orderDto).ConfigureAwait(false);

        return Ok();
    }

    /// <summary>
    /// Details the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async Task<IActionResult> Detail(int id)
    {
        var model = await _orderService.Value.GetDetailsAsync(id).ConfigureAwait(false);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
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
            var dbModel = await _orderService.Value.DeleteAsync(id).ConfigureAwait(false);

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