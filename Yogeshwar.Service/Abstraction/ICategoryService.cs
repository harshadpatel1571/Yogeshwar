namespace Yogeshwar.Service.Abstraction;

public interface ICategoryService : IDisposable
{
    /// <summary>
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <returns></returns>
    internal Task<DataTableResponseCarrier<CategoryDto>> GetByFilterAsync(DataTableFilterDto filterDto);

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<CategoryDto?> GetSingleAsync(int id);

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns></returns>
    ValueTask<int> CreateOrUpdateAsync(CategoryDto category);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    ValueTask<Category?> DeleteAsync(int id);

    /// <summary>
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    ValueTask<bool> DeleteImageAsync(int id);

    /// <summary>
    /// Actives and in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<OneOf<bool, NotFound>> ActiveInActiveRecordAsync(int id);
}