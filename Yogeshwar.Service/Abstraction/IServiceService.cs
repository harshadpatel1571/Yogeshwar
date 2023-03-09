namespace Yogeshwar.Service.Abstraction;

public interface IServiceService : IDisposable
{
    /// <summary>
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <returns></returns>
    internal Task<DataTableResponseCarrier<ServiceDto>> GetByFilterAsync(DataTableFilterDto filterDto);

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <returns></returns>
    Task<int> CreateOrUpdateAsync(ServiceDto service);

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<ServiceDto?> GetSingleAsync(int id);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<int> DeleteAsync(int id);
}
