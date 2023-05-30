namespace Yogeshwar.Service.Service;

/// <summary>
/// Class CachingService.
/// Implements the <see cref="ICachingService" />
/// </summary>
/// <seealso cref="ICachingService" />
[RegisterService(ServiceLifetime.Scoped, typeof(ICachingService))]
internal class CachingService : ICachingService
{
    /// <summary>
    /// The context
    /// </summary>
    private readonly YogeshwarContext _context;

    /// <summary>
    /// The memory cache
    /// </summary>
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// The accessories caching key
    /// </summary>
    private const string AccessoriesCachingKey = "Accessories";

    /// <summary>
    /// The categories caching key
    /// </summary>
    private const string CategoriesCachingKey = "Categories";

    /// <summary>
    /// The configuration caching key
    /// </summary>
    private const string ConfigurationCachingKey = "Configuration";

    /// <summary>
    /// The products caching key
    /// </summary>
    private const string ProductsCachingKey = "Products";    

    /// <summary>
    /// The mapping service
    /// </summary>
    private readonly IMappingService _mappingService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachingService" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="memoryCache">The memory cache.</param>
    /// <param name="mappingService">The mapping service.</param>
    public CachingService(YogeshwarContext context, IMemoryCache memoryCache,
        IMappingService mappingService)
    {
        _context = context;
        _memoryCache = memoryCache;
        _mappingService = mappingService;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    #region Accessories

    /// <summary>
    /// Gets the accessories asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IList&lt;AccessoriesDto&gt;&gt;.</returns>
    async Task<IList<AccessoriesDto>> ICachingService.GetAccessoriesAsync(CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(AccessoriesCachingKey, out var value))
        {
            return await Task.FromResult((List<AccessoriesDto>)value).ConfigureAwait(false);
        }

        return await GetAccessoriesInternalAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Get accessories internal as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;IList`1&gt; representing the asynchronous operation.</returns>
    private async Task<IList<AccessoriesDto>> GetAccessoriesInternalAsync(CancellationToken cancellationToken)
    {
        async Task<List<AccessoriesDto>> FetchData(ICacheEntry x)
        {
            var data = CachingQueryExpression.Accessories(_context, _mappingService);

            var result = await data.ToListAsync(cancellationToken).ConfigureAwait(false);

            x.SetValue(result);

            x.SlidingExpiration = TimeSpan.MaxValue;
            x.AbsoluteExpiration = DateTimeOffset.MaxValue;

            return result;
        }

        return await _memoryCache.GetOrCreateAsync(AccessoriesCachingKey, FetchData).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes the accessories.
    /// </summary>
    void ICachingService.RemoveAccessories()
    {
        _memoryCache.Remove(AccessoriesCachingKey);
    }

    #endregion

    #region Categories

    /// <summary>
    /// Gets the categories asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IList&lt;CategoryDto&gt;&gt;.</returns>
    async Task<IList<CategoryDto>> ICachingService.GetCategoriesAsync(CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(CategoriesCachingKey, out var value))
        {
            return await Task.FromResult((List<CategoryDto>)value).ConfigureAwait(false);
        }

        return await GetCategoriesInternalAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Get categories internal as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;IList`1&gt; representing the asynchronous operation.</returns>
    private async Task<IList<CategoryDto>> GetCategoriesInternalAsync(CancellationToken cancellationToken)
    {
        async Task<List<CategoryDto>> FetchData(ICacheEntry x)
        {
            var data = CachingQueryExpression.Categories(_context, _mappingService);

            var result = await data.ToListAsync(cancellationToken).ConfigureAwait(false);

            x.SetValue(result);

            x.SlidingExpiration = TimeSpan.MaxValue;
            x.AbsoluteExpiration = DateTimeOffset.MaxValue;

            return result;
        }

        return await _memoryCache.GetOrCreateAsync(CategoriesCachingKey, FetchData).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes the categories.
    /// </summary>
    void ICachingService.RemoveCategories()
    {
        _memoryCache.Remove(CategoriesCachingKey);
    }

    #endregion

    #region Products

    /// <summary>
    /// Gets the products asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IList&lt;ProductDto&gt;&gt;.</returns>
    async Task<IList<ProductDto>> ICachingService.GetProductsAsync(CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(ProductsCachingKey, out var value))
        {
            return await Task.FromResult((List<ProductDto>)value).ConfigureAwait(false);
        }

        return await GetProductsInternalAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Get products internal as an asynchronous operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;IList`1&gt; representing the asynchronous operation.</returns>
    private async Task<IList<ProductDto>> GetProductsInternalAsync(CancellationToken cancellationToken)
    {
        async Task<List<ProductDto>> FetchData(ICacheEntry x)
        {
            var data = CachingQueryExpression.Products(_context);

            var result = await data.ToListAsync(cancellationToken).ConfigureAwait(false);

            x.SetValue(result);

            x.SlidingExpiration = TimeSpan.MaxValue;
            x.AbsoluteExpiration = DateTimeOffset.MaxValue;

            return result;
        }

        return await _memoryCache.GetOrCreateAsync(ProductsCachingKey, FetchData).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes the products.
    /// </summary>
    void ICachingService.RemoveProducts()
    {
        _memoryCache.Remove(ProductsCachingKey);
    }

    #endregion

    #region Configuration
    async Task<ConfigurationDto> ICachingService.GetConfigurationSingleAsync(CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(ConfigurationCachingKey, out var value))
        {
            return await Task.FromResult((ConfigurationDto)value).ConfigureAwait(false);
        }

        return await GetConfigurationInternalAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task<ConfigurationDto> GetConfigurationInternalAsync(CancellationToken cancellationToken)
    {
        async Task<ConfigurationDto> FetchData(ICacheEntry x)
        {
            var result = await CachingQueryExpression.configuration(_context, _mappingService);

            x.SetValue(result);

            x.SlidingExpiration = TimeSpan.MaxValue;
            x.AbsoluteExpiration = DateTimeOffset.MaxValue;

            return result;
        }

        return await _memoryCache.GetOrCreateAsync(ConfigurationCachingKey, FetchData).ConfigureAwait(false);
    }

    void ICachingService.RemoveConfiguration()
    {
        _memoryCache.Remove(ConfigurationCachingKey);
    }
    #endregion
}

/// <summary>
/// Class CachingQueryExpression.
/// </summary>
file static class CachingQueryExpression
{
    /// <summary>
    /// The accessories
    /// </summary>
    public static readonly Func<YogeshwarContext, IMappingService, IAsyncEnumerable<AccessoriesDto>> Accessories =
        EF.CompileAsyncQuery(
            (YogeshwarContext context, IMappingService mappingService) =>
                context.Accessories
                    .AsNoTracking()
                    .Where(c => !c.IsDeleted)
                    .Select(c => mappingService.Map(c)));

    /// <summary>
    /// The categories
    /// </summary>
    public static readonly Func<YogeshwarContext, IMappingService, IAsyncEnumerable<CategoryDto>> Categories =
        EF.CompileAsyncQuery(
            (YogeshwarContext context, IMappingService mappingService) =>
                context.Categories
                    .AsNoTracking()
                    .Where(c => !c.IsDeleted)
                    .Select(c => mappingService.Map(c)));

    /// <summary>
    /// The products
    /// </summary>
    public static readonly Func<YogeshwarContext, IAsyncEnumerable<ProductDto>> Products =
        EF.CompileAsyncQuery(
            (YogeshwarContext context) =>
                context.Products
                    .AsNoTracking()
                    .Where(c => !c.IsDeleted)
                    .Include(x => x.ProductCategories)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.ProductAccessories)
                    .ThenInclude(x => x.Accessory)
                    .Include(x => x.ProductImages)
                    .Select(x => new ProductDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price,
                        ModelNo = x.ModelNo,
                        HsnNo = x.HsnNo,
                        Gst = x.Gst,
                        Video = x.Video,
                        CreatedDate = x.CreatedDate,
                        ModifiedDate = x.ModifiedDate,
                        IsActive = x.IsActive,
                        AccessoryIds = x.ProductAccessories.Select(c => c.AccessoryId).ToArray(),
                        CategoryIds = x.ProductCategories.Select(c => c.CategoryId).ToArray(),
                        ProductAccessories = x.ProductAccessories.Select(c => new ProductAccessoryDto
                        {
                            Id = c.Id,
                            AccessoryId = c.AccessoryId,
                            ProductId = c.ProductId,
                            Quantity = c.Quantity,
                            Accessory = new AccessoriesDto
                            {
                                Id = c.Accessory.Id,
                                CreatedDate = c.Accessory.CreatedDate,
                                ModifiedDate = c.Accessory.ModifiedDate,
                                IsActive = c.Accessory.IsActive,
                                Name = c.Accessory.Name,
                                Image = c.Accessory.Image,
                                Quantity = c.Accessory.Quantity,
                                MeasurementType = c.Accessory.MeasurementType,
                                Price = c.Accessory.Price,
                                Description = c.Accessory.Description
                            }
                        }).ToArray(),
                        ProductCategories = x.ProductCategories.Select(c => new ProductCategoryDto
                        {
                            ProductId = c.ProductId,
                            CategoryId = c.CategoryId,
                            Id = c.Id,
                            Category = new CategoryDto
                            {
                                Id = c.Category.Id,
                                Name = c.Category.Name,
                                HsnNo = c.Category.HsnNo,
                                Image = c.Category.Image,
                                CreatedDate = c.Category.CreatedDate,
                                ModifiedDate = c.Category.ModifiedDate,
                                IsActive = c.Category.IsActive,
                            }
                        }).ToArray(),
                        ProductImages = x.ProductImages.Select(c => new ProductImageDto
                        {
                            Id = c.Id,
                            ProductId = c.ProductId,
                            Image = c.Image
                        }).ToArray()
                    }));

    /// <summary>
    /// The configuration
    /// </summary>
    public static readonly Func<YogeshwarContext, IMappingService, Task<ConfigurationDto>> configuration =
        EF.CompileAsyncQuery(
            (YogeshwarContext context, IMappingService mappingService) =>
                context.Configurations
                    .AsNoTracking()
                    .Select(c => mappingService.Map(c))
                    .FirstOrDefault());
}