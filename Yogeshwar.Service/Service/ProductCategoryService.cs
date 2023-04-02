namespace Yogeshwar.Service.Service;

/// <summary>
/// Class ProductCategoryService.
/// Implements the <see cref="IProductCategoryService" />
/// </summary>
/// <seealso cref="IProductCategoryService" />
[RegisterService(ServiceLifetime.Scoped, typeof(IProductCategoryService))]
public class ProductCategoryService : IProductCategoryService
{
    /// <summary>
    /// The context
    /// </summary>
    private readonly YogeshwarContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductCategoryService" /> class.
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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;ProductCategoryDto&gt; representing the asynchronous operation.</returns>
    public async Task<ProductCategoryDto?> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.ProductCategories
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => DtoSelector(x))
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Select the Dto.
    /// </summary>
    /// <param name="productCategory">The product category.</param>
    /// <returns>ProductCategoryDto.</returns>
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
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    public async ValueTask<int> CreateAsync(ProductCategoryDto productCategory, CancellationToken cancellationToken)
    {
        var dbModel = new ProductCategory
        {
            CategoryId = productCategory.CategoryId,
            ProductId = productCategory.ProductId
        };

        await _context.ProductCategories.AddAsync(dbModel, cancellationToken).ConfigureAwait(false);

        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ProductCategory&gt; representing the asynchronous operation.</returns>
    public async ValueTask<ProductCategory?> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.ProductCategories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return null;
        }

        _context.ProductCategories.Remove(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return dbModel;
    }
}