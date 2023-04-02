namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface IProductService
/// Extends the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="IDisposable" />
public interface IProductService : IDisposable
{
    /// <summary>
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;DataTableResponseCarrier&lt;ProductDto&gt;&gt;.</returns>
    internal Task<DataTableResponseCarrier<ProductDto>> GetByFilterAsync(DataTableFilterDto filterDto, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Nullable&lt;ProductDto&gt;&gt;.</returns>
    Task<ProductDto?> GetSingleAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the accessories quantity.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Object&gt;.</returns>
    Task<object> GetAccessoriesQuantity(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Int32&gt;.</returns>
    Task<int> CreateOrUpdateAsync(ProductDto productDto, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Int32&gt;.</returns>
    Task<int> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>ValueTask&lt;System.Boolean&gt;.</returns>
    ValueTask<bool> DeleteImageAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the video asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>ValueTask&lt;System.Boolean&gt;.</returns>
    ValueTask<bool> DeleteVideoAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Actives and in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;OneOf&lt;System.Boolean, NotFound&gt;&gt;.</returns>
    Task<OneOf<bool, NotFound>> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken);
}