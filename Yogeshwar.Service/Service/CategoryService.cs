namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(ICategoryService))]
public class CategoryService : ICategoryService
{
    private readonly YogeshwarContext _context;
    private static string _categoryImageReadPath;
    private readonly string _imageSavePath;

    public CategoryService(YogeshwarContext context, IConfiguration configuration,
        IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _imageSavePath = $"{hostEnvironment.WebRootPath}/DataImages/Category";
        _categoryImageReadPath = configuration["File:ReadPath"] + "/Category";
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    async Task<DataTableResponseCarrier<CategoryDto>> ICategoryService.GetByFilterAsync(
        DataTableFilterDto filterDto)
    {
        var result = _context.Categories.Where(x => !x.IsDeleted).AsNoTracking();

        if (!string.IsNullOrEmpty(filterDto.SearchValue))
        {
            result = result.Where(x => x.Name.Contains(filterDto.SearchValue));
        }

        var model = new DataTableResponseCarrier<CategoryDto>
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

    private static CategoryDto DtoSelector(Category category) =>
        new()
        {
            Id = category.Id,
            Name = category.Name,
            Image = $"{_categoryImageReadPath}/{category.Image}"
        };

    public async Task<CategoryDto?> GetSingleAsync(int id)
    {
        return await _context.Categories.AsNoTracking()
            .Where(x => x.Id == id).Select(x => DtoSelector(x))
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async ValueTask<int> CreateOrUpdateAsync(CategoryDto category)
    {
        if (category.Id < 1)
        {
            return await CreateAsync(category).ConfigureAwait(false);
        }

        return await UpdateAsync(category).ConfigureAwait(false);
    }

    private async ValueTask<int> CreateAsync(CategoryDto category)
    {
        var image = (string?)null;

        if (category.ImageFile is not null)
        {
            image = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                    Path.GetExtension(category.ImageFile.FileName);
            await category.ImageFile.SaveAsync($"{_imageSavePath}/{image}").ConfigureAwait(false);
        }

        var dbModel = new Category
        {
            Name = category.Name,
            Image = image
        };

        await _context.Categories.AddAsync(dbModel).ConfigureAwait(false);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    private async ValueTask<int> UpdateAsync(CategoryDto category)
    {
        var dbModel = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == category.Id);

        if (dbModel is null)
        {
            return 0;
        }

        dbModel.Name = category.Name;

        if (category.ImageFile is not null)
        {
            var path = $"{_imageSavePath}/{dbModel.Image}";

            DeleteFileIfExist(path);

            var image = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                        Path.GetExtension(category.ImageFile.FileName);
            await category.ImageFile.SaveAsync($"{_imageSavePath}/{image}").ConfigureAwait(false);

            dbModel.Image = image;
        }

        _context.Categories.Update(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async ValueTask<Category?> DeleteAsync(int id)
    {
        var dbModel = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id);

        if (dbModel is null)
        {
            return null;
        }

        dbModel.IsDeleted = true;

        _context.Categories.Update(dbModel);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return dbModel;
    }

    private static void DeleteFileIfExist(string name)
    {
        if (File.Exists(name))
        {
            File.Delete(name);
        }
    }

    public async ValueTask<bool> DeleteImageAsync(int id)
    {
        var dbModel = await _context.Categories
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return false;
        }

        var path = $"{_imageSavePath}/{dbModel.Image}";

        DeleteFileIfExist(path);

        dbModel.Image = null;

        _context.Categories.Update(dbModel);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }
}