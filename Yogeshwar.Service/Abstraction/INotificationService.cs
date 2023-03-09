namespace Yogeshwar.Service.Abstraction;

public interface INotificationService : IDisposable
{
    /// <summary>
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <returns></returns>
    internal Task<DataTableResponseCarrier<NotificationDto>> GetByFilterAsync(DataTableFilterDto filterDto);
}
