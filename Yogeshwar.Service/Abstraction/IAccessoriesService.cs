namespace Yogeshwar.Service.Abstraction;

public interface IAccessoriesService : IDisposable
{
    internal Task<DetaTableResponseCarrier<AccessoriesDto>> GetByFilterAsync(DataTableFilterDto filterDto);

    Task<AccessoriesDto?> GetSingleAsync(int id);

    Task<int> CreateOrUpdateAsync(AccessoriesDto customer);

    ValueTask<bool> DeleteImageAsync(int id);

    Task<Accessory?> DeleteAsync(int id);
}