namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(IProductCategoryService))]
public class ProductCategoryService : IProductCategoryService
{
    private readonly YogeshwarContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCategoryService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public ProductCategoryService(YogeshwarContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async Task<ProductCategoryDto?> GetSingleAsync(int id)
    {
        return await _context.ProductCategories
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => DtoSelector(x))
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Select the Dto.
    /// </summary>
    /// <param name="productCategory">The product category.</param>
    /// <returns></returns>
    private static ProductCategoryDto DtoSelector(ProductCategory productCategory) =>
        new()
        {
            Id = productCategory.Id,
            CategoryId = productCategory.CategoryId,
            ProductId = productCategory.ProductId
        };

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="productCategory">The product category.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
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