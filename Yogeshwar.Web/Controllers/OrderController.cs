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

    public IActionResult AddEdit()
    {
        return View();
    }

    public IActionResult Detail()
    {
        return View();
    }
}