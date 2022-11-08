namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(IProductService))]
internal class ProductService : IProductService
{
    private readonly YogeshwarContext _context;
    private static string _productImageReadPath;
    private static string _videoReadPath;
    private static string _accessoriesImageReadPath;
    private readonly string _imageSavePath;
    private readonly string _videoSavePath;

    public ProductService(YogeshwarContext context, IConfiguration configuration,
        IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _productImageReadPath = configuration["File:ReadPath"] + "/Product";
        _videoReadPath = configuration["File:ReadPath"] + "/Product/Video";
        _imageSavePath = $"{hostEnvironment.WebRootPath}/DataImages/Product";
        _videoSavePath = $"{hostEnvironment.WebRootPath}/DataImages/Product/Video";
        _accessoriesImageReadPath = configuration["File:ReadPath"] + "/Accessories";
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    async Task<DataTableResponseCarrier<ProductDto>> IProductService.GetByFilterAsync(DataTableFilterDto filterDto)
    {
        var result = _context.Products.AsNoTracking();

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

        var data = await result.OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder)
            .Select(x => DtoSelector(x)).ToListAsync().ConfigureAwait(false);

        model.Data = data;

        return model;
    }

    public async Task<object> GetAccessoriesQuantity(int id)
    {
        return await _context.ProductAccessories.Where(x => x.ProductId == id)
            .Select(x => new { key = x.Accessories.Name, value = x.Quantity })
            .ToListAsync().ConfigureAwait(false);
    }

    private static ProductDto DtoSelector(Product product) =>
        new()
        {
            Id = product.Id,
            Name = product.Name,
            ModelNo = product.ModelNo,
            Description = product.Description,
            Price = product.Price,
            Video = product.Video != null ? $"{_videoReadPath}/{product.Video}" : null,
            AccessoriesQuantity = product.ProductAccessories.Select(x => new AccessoriesQuantity
            {
                AccessoriesId = x.AccessoriesId,
                Quantity = x.Quantity,
                Image = x.Accessories.Image != null ? $"{_accessoriesImageReadPath}/{x.Accessories.Image}" : null
            }).ToArray(),
            Images = product.ProductImages
                .Select(x => new ImageIds
                {
                    Image = $"{_productImageReadPath}/{x.Image}",
                    Id = x.Id
                }).ToArray(),
            Accessories = product.ProductAccessories.Select(x => x.AccessoriesId)
                .ToArray()
        };

    public async Task<ProductDto?> GetSingleAsync(int id)
    {
        return await _context.Products.AsNoTracking()
            .Where(x => x.Id == id).Include(x => x.ProductAccessories)
            .ThenInclude(x => x.Accessories)
            .Include(x => x.ProductImages)
            .Select(x => DtoSelector(x))
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<int> CreateOrUpdateAsync(ProductDto productDto)
    {
        if (productDto.Id < 1)
        {
            return await CreateAsync(productDto).ConfigureAwait(false);
        }

        return await UpdateAsync(productDto).ConfigureAwait(false);
    }

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
            ProductAccessories = productDto.AccessoriesQuantity.Select(x => new ProductAccessory
            {
                AccessoriesId = x.AccessoriesId,
                Quantity = x.Quantity
            }).ToArray(),
            ProductImages = images.Select(x => new ProductImage
            {
                Image = x
            }).ToArray()
        };

        await _context.Products.AddAsync(dbModel).ConfigureAwait(false);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    private async ValueTask<int> UpdateAsync(ProductDto productDto)
    {
        var dbModel = await _context.Products
            .Include(x => x.ProductAccessories)
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
        dbModel.Price = productDto.Price!.Value;

        var newAccessories = productDto.AccessoriesQuantity
            .Select(x => new ProductAccessory
            {
                ProductId = dbModel.Id,
                AccessoriesId = x.AccessoriesId,
                Quantity = x.Quantity
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
        _context.Products.Update(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    private static void DeleteFileIfExist(string name)
    {
        if (File.Exists(name))
        {
            File.Delete(name);
        }
    }

    public async Task<Product?> DeleteAsync(int id)
    {
        var dbModel = await _context.Products
            .Include(x => x.ProductAccessories)
            .Include(x => x.ProductImages)
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return null;
        }

        _context.ProductAccessories.RemoveRange(dbModel.ProductAccessories);
        _context.ProductImages.RemoveRange(dbModel.ProductImages);
        _context.Products.Remove(dbModel);

        await _context.SaveChangesAsync().ConfigureAwait(false);

        DeleteFileIfExist($"{_videoSavePath}/{dbModel.Video}");

        foreach (var productImage in dbModel.ProductImages)
        {
            DeleteFileIfExist($"{_imageSavePath}/{productImage.Image}");
        }

        return dbModel;
    }

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