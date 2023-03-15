namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(IProductService))]
internal class ProductService : IProductService
{
    private readonly YogeshwarContext _context;
    private readonly IConfiguration _configuration;
    private readonly string _imageSavePath;
    private readonly string _videoSavePath;
    private readonly Lazy<ICurrentUserService> _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="currentUserService">The current user service.</param>
    public ProductService(YogeshwarContext context, IConfiguration configuration,
        IWebHostEnvironment hostEnvironment, Lazy<ICurrentUserService> currentUserService)
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
    /// Gets the by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <returns></returns>
    async Task<DataTableResponseCarrier<ProductDto>> IProductService.GetByFilterAsync(DataTableFilterDto filterDto)
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
            .ToListAsync().ConfigureAwait(false);

        model.Data = data;

        return model;
    }

    /// <summary>
    /// Gets the accessories quantity.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async Task<object> GetAccessoriesQuantity(int id)
    {
        return await _context.ProductAccessories.Where(x => x.ProductId == id)
            .Select(x => new { key = x.Accessories.Name, value = x.Quantity })
            .ToListAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Select the Dto.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
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
            CreatedBy = product.CreatedBy,
            CreatedDate = product.CreatedDate,
            ModifiedBy = product.ModifiedBy,
            ModifiedDate = product.ModifiedDate
        };

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async Task<ProductDto?> GetSingleAsync(int id)
    {
        return await _context.Products.AsNoTracking()
            .Where(x => x.Id == id).Include(x => x.ProductAccessories)
            .ThenInclude(x => x.Accessories)
            .Include(x => x.ProductImages)
            .Select(x => DtoSelector(x, _configuration))
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Creates the or update asynchronous.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <returns></returns>
    public async Task<int> CreateOrUpdateAsync(ProductDto productDto)
    {
        if (productDto.Id < 1)
        {
            return await CreateAsync(productDto).ConfigureAwait(false);
        }

        return await UpdateAsync(productDto).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <returns></returns>
    private async ValueTask<int> CreateAsync(ProductDto productDto)
    {
        var video = (string?)null;
        var images = Array.Empty<string>();

        if (productDto.VideoFile is not null)
        {
            video = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                    Path.GetExtension(productDto.VideoFile.FileName);
            await productDto.VideoFile.SaveAsync($"{_videoSavePath}/{video}").ConfigureAwait(false);
        }

        if (productDto.ImageFiles is { Count: > 0 })
        {
            images = new string[productDto.ImageFiles.Count];

            for (var i = 0; i < images.Length; i++)
            {
                images[i] = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                            Path.GetExtension(productDto.ImageFiles[i].FileName);

                await productDto.ImageFiles[i].SaveAsync($"{_imageSavePath}/{images[i]}").ConfigureAwait(false);
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
            CreatedBy = _currentUserService.Value.GetCurrentUserId(),
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

        await _context.Products.AddAsync(dbModel).ConfigureAwait(false);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Updates the asynchronous.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <returns></returns>
    private async ValueTask<int> UpdateAsync(ProductDto productDto)
    {
        var dbModel = await _context.Products
            .Include(x => x.ProductAccessories)
            .Include(x => x.ProductCategories)
            .FirstOrDefaultAsync(x => x.Id == productDto.Id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        if (productDto.VideoFile is not null)
        {
            var video = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                        Path.GetExtension(productDto.VideoFile.FileName);

            await productDto.VideoFile.SaveAsync($"{_videoSavePath}/{video}").ConfigureAwait(false);

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

                await productDto.ImageFiles[i].SaveAsync($"{_imageSavePath}/{images[i]}").ConfigureAwait(false);
            }
        }

        dbModel.Id = productDto.Id;
        dbModel.Name = productDto.Name;
        dbModel.Description = productDto.Description;
        dbModel.ModelNo = productDto.ModelNo;
        dbModel.IsActive = productDto.IsActive;
        dbModel.ModifiedBy = _currentUserService.Value.GetCurrentUserId();
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

            await _context.ProductImages.AddRangeAsync(newImages).ConfigureAwait(false);
        }

        _context.ProductAccessories.RemoveRange(dbModel.ProductAccessories);
        await _context.ProductAccessories.AddRangeAsync(newAccessories).ConfigureAwait(false);

        _context.ProductCategories.RemoveRange(dbModel.ProductCategories);
        await _context.ProductCategories.AddRangeAsync(newCategories).ConfigureAwait(false);

        _context.Products.Update(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the file if exist.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
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
    /// <returns></returns>
    public async Task<int> DeleteAsync(int id)
    {
        var dbModel = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        dbModel.IsDeleted = true;
        dbModel.ModifiedBy = _currentUserService.Value.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Products.Update(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async ValueTask<bool> DeleteImageAsync(int id)
    {
        var dbModel = await _context.ProductImages
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

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
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }

    /// <summary>
    /// Deletes the video asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async ValueTask<bool> DeleteVideoAsync(int id)
    {
        var dbModel = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

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
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }
}