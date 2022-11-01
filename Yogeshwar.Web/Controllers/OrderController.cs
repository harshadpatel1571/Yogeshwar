namespace Yogeshwar.Web.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly Lazy<IOrderService> _orderService;

    public OrderController(Lazy<IOrderService> orderService)
    {
        _orderService = orderService;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing & _orderService.IsValueCreated)
        {
            _orderService.Value.Dispose();
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

    [HttpPost]
    public async Task<IActionResult> GetAccessoriesDetail([FromQuery] int productId)
    {
        var detail = await _orderService.Value.GetAccessoriesAsync(productId);

        if (detail is null)
        {
            return NotFound();
        }

        return Json(detail);
    }

    public async Task<IActionResult> AddEdit([FromServices] IDropDownService dropDownService)
    {
        using var _ = dropDownService;

        ViewBag.Customers = new SelectList(await dropDownService.BindDropDownForCustomersAsync(),
            "Key", "Text");
        ViewBag.Status = new SelectList(dropDownService.BindDropDownForStatus(),
            "Key", "Text");
        ViewBag.OrderStatus = new SelectList(dropDownService.BindDropDownForOrderStatus(),
            "Key", "Text");
        ViewBag.Products = new SelectList(await dropDownService.BindDropDownForProductsAsync(),
            "Key", "Text");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddEdit(OrderDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetKeyErrors();
            return BadRequest(errors);
        }

        await _orderService.Value.CreateOrUpdateAsync(orderDto);

        return Ok();
    }

    public IActionResult Detail()
    {
        return View();
    }
}