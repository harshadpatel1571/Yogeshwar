namespace Yogeshwar.Service.Abstraction;

public interface ICategoryService : IDisposable
{
    internal Task<DataTableResponseCarrier<CategoryDto>> GetByFilterAsync(DataTableFilterDto filterDto);

    Task<CategoryDto?> GetSingleAsync(int id);

    ValueTask<int> CreateOrUpdateAsync(CategoryDto category);

    ValueTask<Category?> DeleteAsync(int id);

    ValueTask<bool> DeleteImageAsync(int id);
}