namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(ICategoryService))]
public class CategoryService : ICategoryService
{
    private readonly YogeshwarContext _context;
    private readonly IConfiguration _configuration;
    private readonly string _imageSavePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    public CategoryService(YogeshwarContext context, IConfiguration configuration,
        IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _configuration = configuration;
        _imageSavePath = $"{hostEnvironment.WebRootPath}/DataImages/Category";
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
            .Select(x => DtoSelector(x, _configuration)).ToListAsync().ConfigureAwait(false);

        model.Data = data;

        return model;
    }

    /// <summary>
    /// Select the Dto.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    private static CategoryDto DtoSelector(Category category, IConfiguration configuration) =>
        new()
        {
            Id = category.Id,
            Name = category.Name,
            IsActive = category.IsActive,
            Image = category.Image != null ? $"{configuration["File:ReadPath"]}/Category/{category.Image}" : null
        };

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async Task<CategoryDto?> GetSingleAsync(int id)
    {
        return await _context.Categories.AsNoTracking()
            .Where(x => x.Id == id).Select(x => DtoSelector(x, _configuration))
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns></returns>
    public async ValueTask<int> CreateOrUpdateAsync(CategoryDto category)
    {
        if (category.Id < 1)
        {
            return await CreateAsync(category).ConfigureAwait(false);
        }

        return await UpdateAsync(category).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns></returns>
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
            Image = image,
            IsActive = true
        };

        await _context.Categories.AddAsync(dbModel).ConfigureAwait(false);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Updates the asynchronous.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
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
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Actives and in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async Task<OneOf<bool, NotFound>> ActiveInActiveRecordAsync(int id)
    {
        var dbModel = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return new NotFound();
        }

        dbModel.IsActive = !dbModel.IsActive;

        _context.Categories.Update(dbModel);

        await _context.SaveChangesAsync();

        return dbModel.IsActive;
    }
}