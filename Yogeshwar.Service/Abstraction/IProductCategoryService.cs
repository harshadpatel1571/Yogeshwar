namespace Yogeshwar.Service.Abstraction;

public interface IProductCategoryService : IDisposable
{
    Task<ProductCategoryDto?> GetSingleAsync(int id);

    ValueTask<int> CreateAsync(ProductCategoryDto category);

    ValueTask<ProductCategory?> DeleteAsync(int id);
}