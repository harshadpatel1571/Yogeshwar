namespace Yogeshwar.Service.Abstraction;

public interface ICustomerService : IDisposable
{
    internal Task<DetaTableResponseCarrier<CustomerDto>> GetByFilterAsync(DataTableFilterDto filterDto);

    Task<int> CreateOrUpdateAsync(CustomerDto customer);

    Task<CustomerDto?> GetSingleAsync(int id);

    Task<Customer?> DeleteAsync(int id);
}
