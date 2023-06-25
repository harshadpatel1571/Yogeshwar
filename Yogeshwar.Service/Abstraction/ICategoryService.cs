namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface ICategoryService
/// Extends the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="IDisposable" />
public interface ICategoryService : IDisposable
{
    /// <summary>
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;DataTableResponseCarrier&lt;CategoryDto&gt;&gt;.</returns>
    Task<DataTableResponseCarrier<CategoryDto>> GetByFilterAsync(DataTableFilterDto filterDto, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the by identifier asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;CategoryDto&gt;&gt;.</returns>
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates the or update asynchronous.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;CategoryDto&gt;&gt;.</returns>
    Task<CategoryDto?> CreateOrUpdateAsync(CategoryDto category, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;CategoryDto&gt;&gt;.</returns>
    Task<CategoryDto?> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;CategoryDto&gt;&gt;.</returns>
    Task<CategoryDto?> DeleteImageAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Actives the in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;CategoryDto&gt;&gt;.</returns>
    Task<CategoryDto?> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken);
}