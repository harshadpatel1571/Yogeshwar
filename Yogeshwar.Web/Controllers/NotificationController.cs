namespace Yogeshwar.Web.Controllers;

[Authorize]
public class NotificationController : Controller
{
    private readonly Lazy<INotificationService> _notification;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationController"/> class.
    /// </summary>
    /// <param name="notification">The notification.</param>
    public NotificationController(Lazy<INotificationService> notification)
    {
        _notification = notification;
    }

    /// <summary>
    /// Releases all resources currently used by this <see cref="T:Microsoft.AspNetCore.Mvc.Controller" /> instance.
    /// </summary>
    /// <param name="disposing"><c>true</c> if this method is being invoked by the <see cref="M:Microsoft.AspNetCore.Mvc.Controller.Dispose" /> method,
    /// otherwise <c>false</c>.</param>
    protected override void Dispose(bool disposing)
    {
        if (_notification.IsValueCreated & disposing)
        {
            _notification.Value.Dispose();
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

        var data = await _notification.Value.GetByFilterAsync(filters).ConfigureAwait(false);

        var responseModel = new DataTableResponseDto<NotificationDto>
        {
            Draw = filters.Draw,
            Data = data.Data,
            RecordsFiltered = data.TotalCount,
            RecordsTotal = data.TotalCount
        };

        return Json(responseModel);
    }
}
