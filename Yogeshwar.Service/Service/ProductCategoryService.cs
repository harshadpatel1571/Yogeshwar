namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(IProductCategoryService))]
public class ProductCategoryService : IProductCategoryService
{
    private readonly YogeshwarContext _context;

    public ProductCategoryService(YogeshwarContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<ProductCategoryDto?> GetSingleAsync(int id)
    {
        return await _context.ProductCategories.AsNoTracking()
            .Where(x => x.Id == id).Select(x => DtoSelector(x))
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }

    private static ProductCategoryDto DtoSelector(ProductCategory productCategory) =>
        new()
        {
            Id = productCategory.Id,
            CategoryId = productCategory.CategoryId,
            ProductId = productCategory.ProductId
        };


    public async ValueTask<int> CreateAsync(ProductCategoryDto productCategory)
    {
        var dbModel = new ProductCategory
        {
            CategoryId = productCategory.CategoryId,
            ProductId = productCategory.ProductId
        };

        await _context.ProductCategories.AddAsync(dbModel).ConfigureAwait(false);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async ValueTask<ProductCategory?> DeleteAsync(int id)
    {
        var dbModel = await _context.ProductCategories
            .FirstOrDefaultAsync(x => x.Id == id);

        if (dbModel is null)
        {
            return null;
        }

        _context.ProductCategories.Remove(dbModel);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return dbModel;
    }
}