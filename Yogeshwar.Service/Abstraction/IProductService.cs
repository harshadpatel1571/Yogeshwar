namespace Yogeshwar.Service.Abstraction;

public interface IProductService : IDisposable
{
    internal Task<DataTableResponseCarrier<ProductDto>> GetByFilterAsync(DataTableFilterDto filterDto);

    Task<ProductDto?> GetSingleAsync(int id);

    Task<object> GetAccessoriesQuantity(int id);

    Task<int> CreateOrUpdateAsync(ProductDto productDto);

    Task<Product?> DeleteAsync(int id);

    ValueTask<bool> DeleteImageAsync(int id);

    ValueTask<bool> DeleteVideoAsync(int id);
}