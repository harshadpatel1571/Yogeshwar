namespace Yogeshwar.Service.Abstraction;

public interface IProductCategoryService : IDisposable
{
    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<ProductCategoryDto?> GetSingleAsync(int id);

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns></returns>
    ValueTask<int> CreateAsync(ProductCategoryDto category);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    ValueTask<ProductCategory?> DeleteAsync(int id);
}