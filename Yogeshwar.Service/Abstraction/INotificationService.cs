namespace Yogeshwar.Service.Abstraction;

public interface INotificationService : IDisposable
{
    internal Task<DataTableResponseCarrier<NotificationDto>> GetByFilterAsync(DataTableFilterDto filterDto);
}
