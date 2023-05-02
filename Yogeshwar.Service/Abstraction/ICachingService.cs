namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface ICachingService
/// Extends the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="IDisposable" />
internal interface ICachingService : IDisposable
{
    /// <summary>
    /// Gets the accessories asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IList&lt;AccessoriesDto&gt;&gt;.</returns>
    internal Task<IList<AccessoriesDto>> GetAccessoriesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Removes the accessories.
    /// </summary>
    internal void RemoveAccessories();

    /// <summary>
    /// Gets the categories asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IList&lt;CategoryDto&gt;&gt;.</returns>
    internal Task<IList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Removes the categories.
    /// </summary>
    internal void RemoveCategories();

    /// <summary>
    /// Gets the products asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IList&lt;ProductDto&gt;&gt;.</returns>
    internal Task<IList<ProductDto>> GetProductsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Removes the products.
    /// </summary>
    internal void RemoveProducts();

    internal Task<ConfigurationDto> GetConfigurationSingleAsync(CancellationToken cancellationToken);
    internal void RemoveConfiguration();
}