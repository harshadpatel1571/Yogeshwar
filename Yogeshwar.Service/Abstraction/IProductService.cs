namespace Yogeshwar.Service.Abstraction;

public interface IProductService : IDisposable
{
    /// <summary>
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <returns></returns>
    internal Task<DataTableResponseCarrier<ProductDto>> GetByFilterAsync(DataTableFilterDto filterDto);

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<ProductDto?> GetSingleAsync(int id);

    /// <summary>
    /// Gets the accessories quantity.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<object> GetAccessoriesQuantity(int id);

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <returns></returns>
    Task<int> CreateOrUpdateAsync(ProductDto productDto);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<int> DeleteAsync(int id);

    /// <summary>
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    ValueTask<bool> DeleteImageAsync(int id);

    /// <summary>
    /// Deletes the video asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    ValueTask<bool> DeleteVideoAsync(int id);
}