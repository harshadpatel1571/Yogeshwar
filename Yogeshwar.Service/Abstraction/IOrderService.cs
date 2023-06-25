namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface IOrderService
/// Extends the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="IDisposable" />
public interface IOrderService : IDisposable
{
    /// <summary>
    /// Gets the by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;DataTableResponseCarrier&lt;OrderDto&gt;&gt;.</returns>
    Task<DataTableResponseCarrier<OrderDto>> GetByFilterAsync(DataTableFilterDto filterDto,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the by identifier asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;OrderDto&gt;&gt;.</returns>
    Task<OrderDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates the or update asynchronous.
    /// </summary>
    /// <param name="orderDto">The order dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;OneOf&lt;System.Int32, NotFound&gt;&gt;.</returns>
    Task<OneOf<int, NotFound>> CreateOrUpdateAsync(OrderDto orderDto, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;OrderDto&gt;&gt;.</returns>
    Task<OrderDto?> DeleteAsync(int id, CancellationToken cancellationToken);
}