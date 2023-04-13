using Yogeshwar.DB.DbModels;

namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface IProductCategoryService
/// Extends the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="IDisposable" />
public interface IProductCategoryService : IDisposable
{
    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Nullable&lt;ProductCategoryDto&gt;&gt;.</returns>
    Task<ProductCategoryDto?> GetSingleAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>ValueTask&lt;System.Int32&gt;.</returns>
    ValueTask<int> CreateAsync(ProductCategoryDto category, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>ValueTask&lt;System.Nullable&lt;ProductCategory&gt;&gt;.</returns>
    internal ValueTask<int> DeleteAsync(int id, CancellationToken cancellationToken);
}