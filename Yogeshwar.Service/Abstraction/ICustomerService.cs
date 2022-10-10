namespace Yogeshwar.Service.Abstraction;

public interface ICustomerService : IDisposable
{
    internal Task<IList<CustomerDto>> GetByFilterAsync(DataTableFilterDto filterDto);

    Task<int> CreateOrUpdateAsync(CustomerDto customer);

    Task<CustomerDto?> GetSingleAsync(int id);

    Task<Customer?> DeleteAsync(int id);
}
