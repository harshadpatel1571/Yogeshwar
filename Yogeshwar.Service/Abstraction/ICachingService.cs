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
    Task<IList<AccessoriesDto>> GetAccessoriesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Removes the accessories.
    /// </summary>
    void RemoveAccessories();

    /// <summary>
    /// Gets the categories asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IList&lt;CategoryDto&gt;&gt;.</returns>
    Task<IList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Removes the categories.
    /// </summary>
    void RemoveCategories();

    /// <summary>
    /// Gets the products asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IList&lt;ProductDto&gt;&gt;.</returns>
    Task<IList<ProductDto>> GetProductsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Removes the products.
    /// </summary>
    void RemoveProducts();

    /// <summary>
    /// Gets the configurations asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IList&lt;ConfigurationDto&gt;&gt;.</returns>
    Task<IList<ConfigurationDto>> GetConfigurationsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Removes the configurations.
    /// </summary>
    void RemoveConfigurations();
}