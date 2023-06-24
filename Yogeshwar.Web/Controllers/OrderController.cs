//namespace Yogeshwar.Web.Controllers;

///// <summary>
///// Class OrderController.
///// Implements the <see cref="Controller" />
///// </summary>
///// <seealso cref="Controller" />
//[Authorize]
//public class OrderController : Controller
//{
//    /// <summary>
//    /// The order service
//    /// </summary>
//    private readonly Lazy<IOrderService> _orderService;

//    /// <summary>
//    /// Initializes a new instance of the <see cref="OrderController" /> class.
//    /// </summary>
//    /// <param name="orderService">The order service.</param>
//    public OrderController(Lazy<IOrderService> orderService)
//    {
//        _orderService = orderService;
//    }

//    /// <summary>
//    /// Releases all resources currently used by this <see cref="T:Microsoft.AspNetCore.Mvc.Controller" /> instance.
//    /// </summary>
//    /// <param name="disposing"><c>true</c> if this method is being invoked by the <see cref="M:Microsoft.AspNetCore.Mvc.Controller.Dispose" /> method,
//    /// otherwise <c>false</c>.</param>
//    protected override void Dispose(bool disposing)
//    {
//        if (disposing & _orderService.IsValueCreated)
//        {
//            _orderService.Value.Dispose();
//        }

//        base.Dispose(disposing);
//    }

//    /// <summary>
//    /// Index view.
//    /// </summary>
//    /// <returns>IActionResult.</returns>
//    public IActionResult Index()
//    {
//        return View();
//    }

//    /// <summary>
//    /// Binds the data.
//    /// </summary>
//    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
//    /// <returns>IActionResult.</returns>
//    [HttpPost]
//    public async Task<IActionResult> BindData(CancellationToken cancellationToken)
//    {
//        var filters = DataExtractor.Extract(Request);

//        var data = await _orderService.Value.GetByFilterAsync(filters, cancellationToken).ConfigureAwait(false);

//        var responseModel = new DataTableResponseDto<OrderDto>
//        {
//            Draw = filters.Draw,
//            Data = data.Data,
//            RecordsFiltered = data.TotalCount,
//            RecordsTotal = data.TotalCount
//        };

//        return Json(responseModel);
//    }

//    /// <summary>
//    /// Gets the accessories detail.
//    /// </summary>
//    /// <param name="productId">The product identifier.</param>
//    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
//    /// <returns>IActionResult.</returns>
//    //[HttpPost]
//    //public async Task<IActionResult> GetAccessoriesDetail([FromQuery] int productId,
//    //    CancellationToken cancellationToken)
//    //{
//    //    var detail = await _orderService.Value.GetAccessoriesAsync(productId, cancellationToken).ConfigureAwait(false);

//    //    if (detail is null)
//    //    {
//    //        return NotFound();
//    //    }

//    //    return Json(detail);
//    //}


//    /// <summary>
//    /// Add or edit.
//    /// </summary>
//    /// <param name="dropDownService">The drop down service.</param>
//    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
//    /// <returns>IActionResult.</returns>
//    [HttpGet("[controller]/[action]/{customerId:int}")]
//    public async Task<IActionResult> AddEdit(int customerId, [FromServices] IDropDownService dropDownService,
//        CancellationToken cancellationToken)
//    {
//        using var _ = dropDownService;

//        var customers = await dropDownService.BindDropDownForCustomersAsync(cancellationToken, customerId)
//                .ConfigureAwait(false);

//        if (customers.Count < 1)
//        {
//            return NotFound();
//        }

//        ViewBag.Customers = new SelectList(customers, "Key", "Text");
//        ViewBag.Status = new SelectList(dropDownService.BindDropDownForOrderStatus(),
//            "Key", "Text");
//        ViewBag.OrderStatus = new SelectList(dropDownService.BindDropDownForOrderDetailStatus(),
//            "Key", "Text");
//        ViewBag.Products = new SelectList(await dropDownService.BindDropDownForProductsAsync(cancellationToken)
//                .ConfigureAwait(false),
//            "Key", "Text");

//        return View();
//    }

//    /// <summary>
//    /// Add or edit.
//    /// </summary>
//    /// <param name="orderDto">The order dto.</param>
//    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
//    /// <returns>IActionResult.</returns>
//    [HttpPost]
//    public async Task<IActionResult> AddEdit(OrderDto orderDto, CancellationToken cancellationToken)
//    {
//        if (!ModelState.IsValid)
//        {
//            var errors = ModelState.GetKeyErrors();
//            return BadRequest(errors);
//        }

//        await _orderService.Value.CreateOrUpdateAsync(orderDto, cancellationToken).ConfigureAwait(false);

//        return Ok();
//    }

//    /// <summary>
//    /// Details the specified identifier.
//    /// </summary>
//    /// <param name="id">The identifier.</param>
//    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
//    /// <returns>IActionResult.</returns>
//    public async Task<IActionResult> Detail(int id, CancellationToken cancellationToken)
//    {
//        var model = await _orderService.Value
//            .GetDetailsAsync(id, cancellationToken)
//            .ConfigureAwait(false);

//        if (model is null)
//        {
//            return NotFound();
//        }

//        return View(model);
//    }

//    /// <summary>
//    /// Deletes the specified identifier.
//    /// </summary>
//    /// <param name="id">The identifier.</param>
//    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
//    /// <returns>IActionResult.</returns>
//    [HttpPost]
//    public async ValueTask<IActionResult> Delete(int id, CancellationToken cancellationToken)
//    {
//        try
//        {
//            var dbModel = await _orderService.Value
//                .DeleteAsync(id, cancellationToken)
//                .ConfigureAwait(false);

//            if (dbModel is null)
//            {
//                return NotFound();
//            }

//            return NoContent();
//        }
//        catch
//        {
//            return BadRequest();
//        }
//    }
//}