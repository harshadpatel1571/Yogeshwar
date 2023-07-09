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
    /// Get by identifier as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ProductDto&gt; representing the asynchronous operation.</returns>
    public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _cachingService.GetProductsAsync(cancellationToken).ConfigureAwait(false);

        return data.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Create or update as an asynchronous operation.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ProductDto&gt; representing the asynchronous operation.</returns>
    public async Task<ProductDto?> CreateOrUpdateAsync(ProductDto productDto, CancellationToken cancellationToken)
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
    /// <returns>A Task&lt;Yogeshwar.Service.Dto.ProductDto&gt; representing the asynchronous operation.</returns>
    private async Task<ProductDto> CreateAsync(ProductDto productDto, CancellationToken cancellationToken)
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

        var dbModel = new Product
        {
            Name = productDto.Name,
            ModelNo = productDto.ModelNo,
            HsnNo = productDto.HsnNo,
            Description = productDto.Description,
            Price = productDto.Price,
            Gst = productDto.Gst,
            IsActive = true,
            CreatedDate = DateTime.Now,
            CreatedBy = _currentUserService.GetCurrentUserId(),
            Video = video,
            ProductCategories = productDto.CategoryIds
                .Select(x => new ProductCategory
                {
                    CategoryId = x
                }).ToArray(),
            ProductImages = images.Select(x => new ProductImage
            {
                Image = x
            }).ToArray()
        };

        await _context.Products.AddAsync(dbModel, cancellationToken).ConfigureAwait(false);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveProducts();

        return _mappingService.Map(dbModel);
    }

    /// <summary>
    /// Update as an asynchronous operation.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;Yogeshwar.Service.Dto.ProductDto?&gt; representing the asynchronous operation.</returns>
    private async Task<ProductDto?> UpdateAsync(ProductDto productDto, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == productDto.Id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return null;
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
        dbModel.Price = productDto.Price;
        dbModel.HsnNo = productDto.HsnNo;

        var newAccessories = productDto.ProductAccessories
            .Select(x => new ProductAccessory
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
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveProducts();

        dbModel.ProductAccessories = Array.Empty<ProductAccessory>();
        dbModel.ProductCategories = Array.Empty<ProductCategory>();

        return _mappingService.Map(dbModel);
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
    /// Delete as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;Yogeshwar.Service.Dto.ProductDto?&gt; representing the asynchronous operation.</returns>
    public async Task<ProductDto?> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return null;
        }

        dbModel.IsDeleted = true;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Products.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveProducts();

        return _mappingService.Map(dbModel);
    }

    /// <summary>
    /// Delete image as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;Yogeshwar.Service.Dto.ProductImageDto?&gt; representing the asynchronous operation.</returns>
    public async Task<ProductImageDto?> DeleteImageAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.ProductImages
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return null;
        }

        DeleteFileIfExist(_rootPath + dbModel.Image);

        _context.ProductImages.Remove(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveProducts();

        return _mappingService.Map(dbModel);
    }

    /// <summary>
    /// Delete video as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;Yogeshwar.Service.Dto.ProductDto?&gt; representing the asynchronous operation.</returns>
    public async Task<ProductDto?> DeleteVideoAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return null;
        }

        DeleteFileIfExist(_rootPath + dbModel.Video);

        dbModel.Video = null;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Products.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveProducts();

        return _mappingService.Map(dbModel);
    }

    /// <summary>
    /// Active in active record as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;Yogeshwar.Service.Dto.ProductDto?&gt; representing the asynchronous operation.</returns>
    public async Task<ProductDto?> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return null;
        }

        dbModel.IsActive = !dbModel.IsActive;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Products.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveProducts();

        return _mappingService.Map(dbModel);
    }
}