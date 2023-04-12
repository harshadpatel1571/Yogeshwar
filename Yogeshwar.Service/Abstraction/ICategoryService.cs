using Yogeshwar.DB.DbModels;

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
    internal Task<DataTableResponseCarrier<CategoryDto>> GetByFilterAsync(DataTableFilterDto filterDto, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Nullable&lt;CategoryDto&gt;&gt;.</returns>
    Task<CategoryDto?> GetSingleAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>ValueTask&lt;System.Int32&gt;.</returns>
    ValueTask<int> CreateOrUpdateAsync(CategoryDto category, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>ValueTask&lt;System.Nullable&lt;Category&gt;&gt;.</returns>
    internal ValueTask<Category?> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>ValueTask&lt;System.Boolean&gt;.</returns>
    ValueTask<bool> DeleteImageAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Actives and in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;OneOf&lt;System.Boolean, NotFound&gt;&gt;.</returns>
    Task<OneOf<bool, NotFound>> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken);
}