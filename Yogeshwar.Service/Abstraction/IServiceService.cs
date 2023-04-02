namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface IServiceService
/// Extends the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="IDisposable" />
public interface IServiceService : IDisposable
{
    /// <summary>
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;DataTableResponseCarrier&lt;ServiceDto&gt;&gt;.</returns>
    internal Task<DataTableResponseCarrier<ServiceDto>> GetByFilterAsync(DataTableFilterDto filterDto, CancellationToken cancellationToken);

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Int32&gt;.</returns>
    Task<int> CreateOrUpdateAsync(ServiceDto service, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Nullable&lt;ServiceDto&gt;&gt;.</returns>
    Task<ServiceDto?> GetSingleAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Int32&gt;.</returns>
    Task<int> DeleteAsync(int id, CancellationToken cancellationToken);
}
