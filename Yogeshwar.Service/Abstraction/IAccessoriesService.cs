namespace Yogeshwar.Service.Abstraction;

public interface IAccessoriesService : IDisposable
{
    /// <summary>
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <returns></returns>
    internal Task<DataTableResponseCarrier<AccessoriesDto>> GetByFilterAsync(DataTableFilterDto filterDto);

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<AccessoriesDto?> GetSingleAsync(int id);

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns></returns>
    Task<int> CreateOrUpdateAsync(AccessoriesDto customer);

    /// <summary>
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    ValueTask<bool> DeleteImageAsync(int id);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<int> DeleteAsync(int id);
    
    /// <summary>
    /// Actives and in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<OneOf<bool, NotFound>> ActiveInActiveRecordAsync(int id);
}