using Yogeshwar.DB.DbModels;

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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;DataTableResponseCarrier&lt;OrderDto&gt;&gt;.</returns>
    internal Task<DataTableResponseCarrier<OrderDto>> GetByFilterAsync(DataTableFilterDto filterDto, CancellationToken cancellationToken);

    /// <summary>
    /// Creates the or update asynchronous.
    /// </summary>
    /// <param name="orderDto">The order dto.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;OneOf&lt;System.Int32, NotFound, System.String[]&gt;&gt;.</returns>
    Task<OneOf<int, NotFound, string[]>> CreateOrUpdateAsync(OrderDto orderDto, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the details asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Nullable&lt;OrderDetailViewModel&gt;&gt;.</returns>
    Task<OrderDetailViewModel?> GetDetailsAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the accessories asynchronous.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Nullable&lt;ProductAccessoriesDetailDto&gt;&gt;.</returns>
    internal Task<ProductAccessoriesDetailDto?> GetAccessoriesAsync(int productId, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Nullable&lt;Order&gt;&gt;.</returns>
    internal Task<Order?> DeleteAsync(int id, CancellationToken cancellationToken);
}