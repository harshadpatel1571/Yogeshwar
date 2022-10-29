using Yogeshwar.DB.Models;

namespace Yogeshwar.Service.Abstraction;

public interface IServiceService : IDisposable
{
    internal Task<DataTableResponseCarrier<ServiceDto>> GetByFilterAsync(DataTableFilterDto filterDto);

    Task<int> CreateOrUpdateAsync(ServiceDto service);

    Task<ServiceDto?> GetSingleAsync(int id);

    Task<CustomerService?> DeleteAsync(int id);
}
