using Microsoft.AspNetCore.Mvc;

namespace Yogeshwar.Web.Controllers;

[Authorize]
public class NotificationController : Controller
{
    private readonly Lazy<INotificationService> _notification;

    public NotificationController(Lazy<INotificationService> notification)
    {
        _notification = notification;
    }

    protected override void Dispose(bool disposing)
    {
        if (_notification.IsValueCreated & disposing)
        {
            _notification.Value.Dispose();
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
