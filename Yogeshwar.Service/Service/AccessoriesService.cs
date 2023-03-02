namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(IAccessoriesService))]
internal class AccessoriesService : IAccessoriesService
{
    private readonly YogeshwarContext _context;
    private static string _readPath;
    private readonly string _savePath;

    public AccessoriesService(YogeshwarContext context, IConfiguration configuration,
        IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _readPath = configuration["File:ReadPath"] + "/Accessories";
        _savePath = $"{hostEnvironment.WebRootPath}/DataImages/Accessories";
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    async Task<DataTableResponseCarrier<AccessoriesDto>> IAccessoriesService.GetByFilterAsync(
        DataTableFilterDto filterDto)
    {
        var result = _context.Accessories.AsNoTracking();

        if (!string.IsNullOrEmpty(filterDto.SearchValue))
        {
            result = result.Where(x => x.Name.Contains(filterDto.SearchValue));
        }

        var model = new DataTableResponseCarrier<AccessoriesDto>
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

    private static AccessoriesDto DtoSelector(Accessory accessory) =>
        new()
        {
            Id = accessory.Id,
            Name = accessory.Name,
            Description = accessory.Description,
            Image = accessory.Image == null ? null : $"{_readPath}/{accessory.Image}",
            Quantity = accessory.Quantity
        };

    public async Task<AccessoriesDto?> GetSingleAsync(int id)
    {
        return await _context.Accessories.AsNoTracking()
            .Where(x => x.Id == id).Select(x => DtoSelector(x))
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<int> CreateOrUpdateAsync(AccessoriesDto customer)
    {
        if (customer.Id < 1)
        {
            return await CreateAsync(customer).ConfigureAwait(false);
        }

        return await UpdateAsync(customer).ConfigureAwait(false);
    }

    private async ValueTask<int> CreateAsync(AccessoriesDto accessory)
    {
        var image = (string?)null;

        if (accessory.File is not null)
        {
            image = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                    Path.GetExtension(accessory.File.FileName);
            await accessory.File.SaveAsync($"{_savePath}/{image}").ConfigureAwait(false);
        }

        var dbModel = new Accessory
        {
            Id = accessory.Id,
            Name = accessory.Name,
            Description = accessory.Description,
            Image = image,
            Quantity = accessory.Quantity
        };

        await _context.Accessories.AddAsync(dbModel).ConfigureAwait(false);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    private async ValueTask<int> UpdateAsync(AccessoriesDto accessory)
    {
        var dbModel = await _context.Accessories
            .FirstOrDefaultAsync(x => x.Id == accessory.Id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        if (accessory.File is not null)
        {
            var image = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                        Path.GetExtension(accessory.File.FileName);
            await accessory.File.SaveAsync($"{_savePath}/{image}").ConfigureAwait(false);

            DeleteImageIfExist($"{_savePath}/{dbModel.Image}");

            dbModel.Image = image;
        }

        dbModel.Id = accessory.Id;
        dbModel.Name = accessory.Name;
        dbModel.Description = accessory.Description;
        dbModel.Quantity = accessory.Quantity;

        _context.Accessories.Update(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    private static void DeleteImageIfExist(string name)
    {
        if (File.Exists(name))
        {
            File.Delete(name);
        }
    }

    public async Task<Accessory?> DeleteAsync(int id)
    {
        var dbModel = await _context.Accessories
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return null;
        }

        _context.Accessories.Remove(dbModel);

        await _context.SaveChangesAsync().ConfigureAwait(false);

        DeleteImageIfExist($"{_savePath}/{dbModel.Image}");

        return dbModel;
    }

    public async ValueTask<bool> DeleteImageAsync(int id)
    {
        var dbModel = await _context.Accessories
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return false;
        }

        var path = $"{_savePath}/{dbModel.Image}";

        DeleteImageIfExist(path);

        dbModel.Image = null;

        await _context.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }
}