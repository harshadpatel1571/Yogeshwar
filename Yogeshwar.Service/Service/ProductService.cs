namespace Yogeshwar.Service.Service;

/// <summary>
/// Class ProductService.
/// Implements the <see cref="IProductService" />
/// </summary>
/// <seealso cref="IProductService" />
[RegisterService(ServiceLifetime.Scoped, typeof(IProductService))]
internal sealed class ProductService : IProductService
{
    /// <summary>
    /// The context
    /// </summary>
    private readonly YogeshwarContext _context;

    /// <summary>
    /// The caching service
    /// </summary>
    private readonly ICachingService _cachingService;

    /// <summary>
    /// The mapping service
    /// </summary>
    private readonly IMappingService _mappingService;

    /// <summary>
    /// The root path
    /// </summary>
    private readonly string _rootPath;

    /// <summary>
    /// The prefix image path
    /// </summary>
    private const string PrefixImagePath = "/DataImages/Product/";

    /// <summary>
    /// The prefix video path
    /// </summary>
    private const string PrefixVideoPath = "/DataImages/Product/Video/";

    /// <summary>
    /// The current user service
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="currentUserService">The current user service.</param>
    /// <param name="cachingService">The caching service.</param>
    /// <param name="mappingService">The mapping service.</param>
    public ProductService(YogeshwarContext context, IWebHostEnvironment hostEnvironment,
        ICurrentUserService currentUserService, ICachingService cachingService,
        IMappingService mappingService)
    {
        _context = context;
        _cachingService = cachingService;
        _mappingService = mappingService;
        _currentUserService = currentUserService;
        _rootPath = hostEnvironment.WebRootPath;
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
    /// Get by filter as an asynchronous operation.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;Yogeshwar.Service.Dto.DataTableResponseCarrier&lt;Yogeshwar.Service.Dto.ProductDto>&gt; representing the asynchronous operation.</returns>
    async Task<DataTableResponseCarrier<ProductDto>> IProductService.GetByFilterAsync(DataTableFilterDto filterDto,
        CancellationToken cancellationToken)
    {
        var cachedData = await _cachingService.GetProductsAsync(cancellationToken).ConfigureAwait(false);

        var result = cachedData.AsQueryable();

        if (!string.IsNullOrEmpty(filterDto.SearchValue))
        {
            result = result.Where(x => x.Name.Contains(filterDto.SearchValue) ||
                                       x.ModelNo.Contains(filterDto.SearchValue));
        }

        var model = new DataTableResponseCarrier<ProductDto>
        {
            TotalCount = result.Count()
        };

        result = result.Skip(filterDto.Skip);

        if (filterDto.Take != -1)
        {
            result = result.Take(filterDto.Take);
        }

        var data = result
            .OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder)
            .ToArray();

        model.Data = data;

        return model;
    }

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;Yogeshwar.Service.Dto.ProductDto?&gt; representing the asynchronous operation.</returns>
    public async Task<ProductDto?> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _cachingService.GetProductsAsync(cancellationToken).ConfigureAwait(false);

        return data.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Creates the or update asynchronous.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;int&gt; representing the asynchronous operation.</returns>
    public async Task<int> CreateOrUpdateAsync(ProductDto productDto, CancellationToken cancellationToken)
    {
        if (productDto.Id < 1)
        {
            return await CreateAsync(productDto, cancellationToken).ConfigureAwait(false);
        }

        return await UpdateAsync(productDto, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Create as an asynchronous operation.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A System.Threading.Tasks.ValueTask&lt;int&gt; representing the asynchronous operation.</returns>
    private async ValueTask<int> CreateAsync(ProductDto productDto, CancellationToken cancellationToken)
    {
        var video = (string?)null;
        var images = Array.Empty<string>();

        if (productDto.VideoFile is not null)
        {
            video = PrefixVideoPath +
                    Guid.NewGuid().ToString().Replace("-", "") +
                    Path.GetExtension(productDto.VideoFile.FileName);

            await productDto.VideoFile.SaveAsync(_rootPath + video, cancellationToken)
                .ConfigureAwait(false);
        }

        if (productDto.ImageFiles is { Count: > 0 })
        {
            images = new string[productDto.ImageFiles.Count];

            for (var i = 0; i < images.Length; i++)
            {
                images[i] = PrefixImagePath +
                            Guid.NewGuid().ToString().Replace("-", "") +
                            Path.GetExtension(productDto.ImageFiles[i].FileName);

                await productDto.ImageFiles[i].SaveAsync(_rootPath + images[i], cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        var dbModel = _mappingService.Map(productDto);

        dbModel.IsActive = true;
        dbModel.CreatedDate = DateTime.Now;
        dbModel.CreatedBy = _currentUserService.GetCurrentUserId();
        dbModel.Video = video;

        dbModel.ProductCategories = productDto.CategoryIds
            .Select(x => new ProductCategory
            {
                CategoryId = x
            }).ToArray();

        dbModel.ProductImages = images.Select(x => new ProductImage
        {
            Image = x
        }).ToArray();

        await _context.Products.AddAsync(dbModel, cancellationToken).ConfigureAwait(false);
        var count = await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveProducts();

        return count;
    }

    /// <summary>
    /// Update as an asynchronous operation.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A System.Threading.Tasks.ValueTask&lt;int&gt; representing the asynchronous operation.</returns>
    private async ValueTask<int> UpdateAsync(ProductDto productDto, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == productDto.Id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        if (productDto.VideoFile is not null)
        {
            var video = PrefixVideoPath +
                        Guid.NewGuid().ToString().Replace("-", "") +
                        Path.GetExtension(productDto.VideoFile.FileName);

            await productDto.VideoFile.SaveAsync(_rootPath + video, cancellationToken).ConfigureAwait(false);

            DeleteFileIfExist(_rootPath + dbModel.Video);

            dbModel.Video = video;
        }

        var images = Array.Empty<string>();

        if (productDto.ImageFiles is { Count: > 0 })
        {
            images = new string[productDto.ImageFiles.Count];

            for (var i = 0; i < images.Length; i++)
            {
                images[i] = PrefixImagePath +
                            Guid.NewGuid().ToString().Replace("-", "") +
                            Path.GetExtension(productDto.ImageFiles[i].FileName);

                await productDto.ImageFiles[i].SaveAsync(_rootPath + images[i], cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        dbModel.Id = productDto.Id;
        dbModel.Name = productDto.Name;
        dbModel.Description = productDto.Description;
        dbModel.ModelNo = productDto.ModelNo;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;
        dbModel.Price = productDto.Price!.Value;
        dbModel.HsnNo = productDto.HsnNo;

        var newAccessories = productDto.ProductAccessories
            .Select(x=> new ProductAccessory
            {
                AccessoryId = x.AccessoryId,
                ProductId = dbModel.Id,
                Quantity = x.Quantity
            });

        var newCategories = productDto.CategoryIds
            .Select(x => new ProductCategory
            {
                ProductId = dbModel.Id,
                CategoryId = x
            });

        if (images.Length > 0)
        {
            var newImages = images.Select(x => new ProductImage
            {
                ProductId = dbModel.Id,
                Image = x
            });

            _context.ProductImages.AddRange(newImages);
        }

        await _context.ProductAccessories
            .Where(x => x.ProductId == dbModel.Id)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);

        await _context.ProductCategories
            .Where(x => x.ProductId == dbModel.Id)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);

        _context.ProductAccessories.AddRange(newAccessories);
        _context.ProductCategories.AddRange(newCategories);

        _context.Products.Update(dbModel);
        var count = await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveProducts();

        return count;
    }

    /// <summary>
    /// Deletes the file if exist.
    /// </summary>
    /// <param name="name">The name.</param>
    private static void DeleteFileIfExist(string name)
    {
        if (File.Exists(name))
        {
            File.Delete(name);
        }
    }

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;int&gt; representing the asynchronous operation.</returns>
    public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        dbModel.IsDeleted = true;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Products.Update(dbModel);
        var count = await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveProducts();

        return count;
    }

    /// <summary>
    /// Delete image as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A System.Threading.Tasks.ValueTask&lt;bool&gt;&gt; representing the asynchronous operation.</returns>
    public async ValueTask<bool> DeleteImageAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.ProductImages
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return false;
        }

        DeleteFileIfExist(_rootPath + dbModel.Image);

        _context.ProductImages.Remove(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveProducts();

        return true;
    }

    /// <summary>
    /// Delete video as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A System.Threading.Tasks.ValueTask&lt;bool&gt; representing the asynchronous operation.</returns>
    public async ValueTask<bool> DeleteVideoAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return false;
        }

        DeleteFileIfExist(_rootPath + dbModel.Video);

        dbModel.Video = null;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Products.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveProducts();

        return true;
    }

    /// <summary>
    /// Active in active record as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;OneOf.OneOf&lt;bool, OneOf.Types.NotFound&gt;&gt; representing the asynchronous operation.</returns>
    public async Task<OneOf<bool, NotFound>> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return new NotFound();
        }

        dbModel.IsActive = !dbModel.IsActive;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Products.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveProducts();

        return dbModel.IsActive;
    }
}