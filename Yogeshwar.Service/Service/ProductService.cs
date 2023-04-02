namespace Yogeshwar.Service.Service;

/// <summary>
/// Class ProductService.
/// Implements the <see cref="IProductService" />
/// </summary>
/// <seealso cref="IProductService" />
[RegisterService(ServiceLifetime.Scoped, typeof(IProductService))]
internal class ProductService : IProductService
{
    /// <summary>
    /// The context
    /// </summary>
    private readonly YogeshwarContext _context;
    /// <summary>
    /// The configuration
    /// </summary>
    private readonly IConfiguration _configuration;
    /// <summary>
    /// The image save path
    /// </summary>
    private readonly string _imageSavePath;
    /// <summary>
    /// The video save path
    /// </summary>
    private readonly string _videoSavePath;
    /// <summary>
    /// The current user service
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="currentUserService">The current user service.</param>
    public ProductService(YogeshwarContext context, IConfiguration configuration,
        IWebHostEnvironment hostEnvironment, ICurrentUserService currentUserService)
    {
        _context = context;
        _configuration = configuration;
        _currentUserService = currentUserService;
        _imageSavePath = $"{hostEnvironment.WebRootPath}/DataImages/Product";
        _videoSavePath = $"{hostEnvironment.WebRootPath}/DataImages/Product/Video";
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
    /// <returns>A Task&lt;Yogeshwar.Service.Dto.DataTableResponseCarrier<Yogeshwar.Service.Dto.ProductDto>&gt; representing the asynchronous operation.</returns>
    /// <font color="red">Badly formed XML comment.</font>
    async Task<DataTableResponseCarrier<ProductDto>> IProductService.GetByFilterAsync(DataTableFilterDto filterDto,
        CancellationToken cancellationToken)
    {
        var result = _context.Products.Where(x => !x.IsDeleted).AsNoTracking();

        if (!string.IsNullOrEmpty(filterDto.SearchValue))
        {
            result = result.Where(x => x.Name.Contains(filterDto.SearchValue) ||
                                       x.ModelNo.Contains(filterDto.SearchValue));
        }

        var model = new DataTableResponseCarrier<ProductDto>
        {
            TotalCount = result.Count(),
        };

        result = result.Skip(filterDto.Skip);

        if (filterDto.Take != -1)
        {
            result = result.Take(filterDto.Take);
        }

        var data = await result
            .OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder)
            .Select(x => DtoSelector(x, _configuration))
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        model.Data = data;

        return model;
    }

    /// <summary>
    /// Gets the accessories quantity.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>object.</returns>
    public async Task<object> GetAccessoriesQuantity(int id, CancellationToken cancellationToken)
    {
        return await _context.ProductAccessories.Where(x => x.ProductId == id)
            .Select(x => new { key = x.Accessories.Name, value = x.Quantity })
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Select the Dto.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>Yogeshwar.Service.Dto.ProductDto.</returns>
    private static ProductDto DtoSelector(Product product, IConfiguration configuration) =>
        new()
        {
            Id = product.Id,
            Name = product.Name,
            ModelNo = product.ModelNo,
            Description = product.Description,
            Price = product.Price,
            Video = product.Video != null ? $"{configuration["File:ReadPath"]}/Product/Video/{product.Video}" : null,
            AccessoriesQuantity = product.ProductAccessories.Select(x => new AccessoriesQuantity
            {
                AccessoriesId = x.AccessoriesId,
                Quantity = x.Quantity,
                Image = x.Accessories.Image != null
                    ? $"{configuration["File:ReadPath"]}/Accessories/{x.Accessories.Image}"
                    : null
            }).ToArray(),
            Images = product.ProductImages
                .Select(x => new ImageIds
                {
                    Image = $"{configuration["File:ReadPath"]}/Product/{x.Image}",
                    Id = x.Id
                }).ToArray(),
            Accessories = product.ProductAccessories.Select(x => x.AccessoriesId)
                .ToArray(),
            Categories = product.ProductCategories.Select(x => x.CategoryId)
                .ToArray(),
            IsActive = product.IsActive,
            CreatedDate = product.CreatedDate,
            ModifiedDate = product.ModifiedDate
        };

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;Yogeshwar.Service.Dto.ProductDto?&gt; representing the asynchronous operation.</returns>
    public async Task<ProductDto?> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Products.AsNoTracking()
            .Where(x => x.Id == id).Include(x => x.ProductAccessories)
            .ThenInclude(x => x.Accessories)
            .Include(x => x.ProductImages)
            .Select(x => DtoSelector(x, _configuration))
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
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
            return await CreateAsync(productDto,cancellationToken).ConfigureAwait(false);
        }

        return await UpdateAsync(productDto,cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Create as an asynchronous operation.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Threading.Tasks.ValueTask<int>&gt; representing the asynchronous operation.</returns>
    /// <font color="red">Badly formed XML comment.</font>
    private async ValueTask<int> CreateAsync(ProductDto productDto, CancellationToken cancellationToken)
    {
        var video = (string?)null;
        var images = Array.Empty<string>();

        if (productDto.VideoFile is not null)
        {
            video = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                    Path.GetExtension(productDto.VideoFile.FileName);
            await productDto.VideoFile.SaveAsync($"{_videoSavePath}/{video}",cancellationToken).ConfigureAwait(false);
        }

        if (productDto.ImageFiles is { Count: > 0 })
        {
            images = new string[productDto.ImageFiles.Count];

            for (var i = 0; i < images.Length; i++)
            {
                images[i] = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                            Path.GetExtension(productDto.ImageFiles[i].FileName);

                await productDto.ImageFiles[i].SaveAsync($"{_imageSavePath}/{images[i]}",cancellationToken).ConfigureAwait(false);
            }
        }

        var dbModel = new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Description = productDto.Description,
            Video = video,
            ModelNo = productDto.ModelNo,
            Price = productDto.Price!.Value,
            IsActive = true,
            CreatedDate = DateTime.Now,
            CreatedBy = _currentUserService.GetCurrentUserId(),
            ProductAccessories = productDto.AccessoriesQuantity.Select(x => new ProductAccessory
            {
                AccessoriesId = x.AccessoriesId,
                Quantity = x.Quantity
            }).ToArray(),
            ProductCategories = productDto.Categories.Select(x => new ProductCategory
            {
                CategoryId = x,
            }).ToArray(),
            ProductImages = images.Select(x => new ProductImage
            {
                Image = x
            }).ToArray()
        };

        await _context.Products.AddAsync(dbModel,cancellationToken).ConfigureAwait(false);

        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Update as an asynchronous operation.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Threading.Tasks.ValueTask<int>&gt; representing the asynchronous operation.</returns>
    /// <font color="red">Badly formed XML comment.</font>
    private async ValueTask<int> UpdateAsync(ProductDto productDto, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Products
            .Include(x => x.ProductAccessories)
            .Include(x => x.ProductCategories)
            .FirstOrDefaultAsync(x => x.Id == productDto.Id,cancellationToken).ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        if (productDto.VideoFile is not null)
        {
            var video = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                        Path.GetExtension(productDto.VideoFile.FileName);

            await productDto.VideoFile.SaveAsync($"{_videoSavePath}/{video}",cancellationToken).ConfigureAwait(false);

            DeleteFileIfExist($"{_videoSavePath}/{dbModel.Video}");

            dbModel.Video = video;
        }

        var images = Array.Empty<string>();

        if (productDto.ImageFiles is { Count: > 0 })
        {
            images = new string[productDto.ImageFiles.Count];

            for (var i = 0; i < images.Length; i++)
            {
                images[i] = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                            Path.GetExtension(productDto.ImageFiles[i].FileName);

                await productDto.ImageFiles[i].SaveAsync($"{_imageSavePath}/{images[i]}",cancellationToken).ConfigureAwait(false);
            }
        }

        dbModel.Id = productDto.Id;
        dbModel.Name = productDto.Name;
        dbModel.Description = productDto.Description;
        dbModel.ModelNo = productDto.ModelNo;
        dbModel.IsActive = productDto.IsActive;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;
        dbModel.Price = productDto.Price!.Value;

        var newAccessories = productDto.AccessoriesQuantity
            .Select(x => new ProductAccessory
            {
                ProductId = dbModel.Id,
                AccessoriesId = x.AccessoriesId,
                Quantity = x.Quantity
            });

        var newCategories = productDto.Categories
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

            await _context.ProductImages.AddRangeAsync(newImages,cancellationToken).ConfigureAwait(false);
        }

        _context.ProductAccessories.RemoveRange(dbModel.ProductAccessories);
        await _context.ProductAccessories.AddRangeAsync(newAccessories,cancellationToken)
            .ConfigureAwait(false);

        _context.ProductCategories.RemoveRange(dbModel.ProductCategories);
        await _context.ProductCategories.AddRangeAsync(newCategories,cancellationToken)
            .ConfigureAwait(false);

        _context.Products.Update(dbModel);

        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
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
            .FirstOrDefaultAsync(x => x.Id == id,cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        dbModel.IsDeleted = true;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Products.Update(dbModel);

        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Delete image as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Threading.Tasks.ValueTask<bool>&gt; representing the asynchronous operation.</returns>
    /// <font color="red">Badly formed XML comment.</font>
    public async ValueTask<bool> DeleteImageAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.ProductImages
            .FirstOrDefaultAsync(x => x.Id == id,cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return false;
        }

        var path = $"{_imageSavePath}/{dbModel.Image}";

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        _context.ProductImages.Remove(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }

    /// <summary>
    /// Delete video as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Threading.Tasks.ValueTask<bool>&gt; representing the asynchronous operation.</returns>
    /// <font color="red">Badly formed XML comment.</font>
    public async ValueTask<bool> DeleteVideoAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id,cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return false;
        }

        var path = $"{_videoSavePath}/{dbModel.Video}";

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        dbModel.Video = null;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;
        
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }

    /// <summary>
    /// Active in active record as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;OneOf.OneOf<bool, OneOf.Types.NotFound>&gt; representing the asynchronous operation.</returns>
    /// <font color="red">Badly formed XML comment.</font>
    public async Task<OneOf<bool, NotFound>> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id,cancellationToken)
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

        return dbModel.IsActive;
    }
}