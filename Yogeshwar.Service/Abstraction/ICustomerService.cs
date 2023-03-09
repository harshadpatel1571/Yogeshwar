namespace Yogeshwar.Service.Abstraction;

public interface ICustomerService : IDisposable
{
    /// <summary>
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <returns></returns>
    internal Task<DataTableResponseCarrier<CustomerDto>> GetByFilterAsync(DataTableFilterDto filterDto);

    /// <summary>
    /// Creates the or update asynchronous.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns></returns>
    Task<int> CreateOrUpdateAsync(CustomerDto customer);

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<CustomerDto?> GetSingleAsync(int id);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    Task<int> DeleteAsync(int id);
}
