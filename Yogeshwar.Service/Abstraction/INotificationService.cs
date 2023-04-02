namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface INotificationService
/// Extends the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="IDisposable" />
public interface INotificationService : IDisposable
{
    /// <summary>
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <returns>Task&lt;DataTableResponseCarrier&lt;NotificationDto&gt;&gt;.</returns>
    internal Task<DataTableResponseCarrier<NotificationDto>> GetByFilterAsync(DataTableFilterDto filterDto);
}
