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
    Task<DataTableResponseCarrier<ProductDto>> GetByFilterAsync(DataTableFilterDto filterDto, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the by identifier asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;ProductDto&gt;&gt;.</returns>
    Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates the or update asynchronous.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;ProductDto&gt;&gt;.</returns>
    Task<ProductDto?> CreateOrUpdateAsync(ProductDto productDto, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;ProductDto&gt;&gt;.</returns>
    Task<ProductDto?> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;ProductImageDto&gt;&gt;.</returns>
    Task<ProductImageDto?> DeleteImageAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the video asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;ProductDto&gt;&gt;.</returns>
    Task<ProductDto?> DeleteVideoAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Actives the in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;ProductDto&gt;&gt;.</returns>
    Task<ProductDto?> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken);
}