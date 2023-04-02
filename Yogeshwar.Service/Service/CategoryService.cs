namespace Yogeshwar.Service.Service;

/// <summary>
/// Class CategoryService.
/// Implements the <see cref="ICategoryService" />
/// </summary>
/// <seealso cref="ICategoryService" />
[RegisterService(ServiceLifetime.Scoped, typeof(ICategoryService))]
public class CategoryService : ICategoryService
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
    /// The current user service
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// The image save path
    /// </summary>
    private readonly string _imageSavePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryService" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="currentUserService">The current user service.</param>
    public CategoryService(YogeshwarContext context, IConfiguration configuration,
        IWebHostEnvironment hostEnvironment, ICurrentUserService currentUserService)
    {
        _context = context;
        _configuration = configuration;
        _currentUserService = currentUserService;
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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;DataTableResponseCarrier&lt;CategoryDto&gt;&gt;.</returns>
    async Task<DataTableResponseCarrier<CategoryDto>> ICategoryService.GetByFilterAsync(
        DataTableFilterDto filterDto, CancellationToken cancellationToken)
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
            .Select(x => DtoSelector(x, _configuration))
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        model.Data = data;

        return model;
    }

    /// <summary>
    /// Select the Dto.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>CategoryDto.</returns>
    private static CategoryDto DtoSelector(Category category, IConfiguration configuration) =>
        new()
        {
            Id = category.Id,
            Name = category.Name,
            IsActive = category.IsActive,
            Image = category.Image != null ? $"{configuration["File:ReadPath"]}/Category/{category.Image}" : null,
            CreatedDate = category.CreatedDate,
            ModifiedDate = category.ModifiedDate
        };

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;CategoryDto&gt; representing the asynchronous operation.</returns>
    public async Task<CategoryDto?> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Categories.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => DtoSelector(x, _configuration))
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    public async ValueTask<int> CreateOrUpdateAsync(CategoryDto category, CancellationToken cancellationToken)
    {
        if (category.Id < 1)
        {
            return await CreateAsync(category, cancellationToken).ConfigureAwait(false);
        }

        return await UpdateAsync(category, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    private async ValueTask<int> CreateAsync(CategoryDto category, CancellationToken cancellationToken)
    {
        var image = (string?)null;

        if (category.ImageFile is not null)
        {
            image = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                    Path.GetExtension(category.ImageFile.FileName);
            await category.ImageFile.SaveAsync($"{_imageSavePath}/{image}", cancellationToken).ConfigureAwait(false);
        }

        var dbModel = new Category
        {
            Name = category.Name,
            Image = image,
            IsActive = true,
            CreatedDate = DateTime.Now,
            CreatedBy = _currentUserService.GetCurrentUserId()
        };

        await _context.Categories.AddAsync(dbModel, cancellationToken).ConfigureAwait(false);

        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates the asynchronous.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    private async ValueTask<int> UpdateAsync(CategoryDto category, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == category.Id, cancellationToken).ConfigureAwait(false);

        if (dbModel is null)
        {
            return 0;
        }

        dbModel.Name = category.Name;
        dbModel.ModifiedDate = DateTime.Now;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();

        if (category.ImageFile is not null)
        {
            var path = $"{_imageSavePath}/{dbModel.Image}";

            DeleteFileIfExist(path);

            var image = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                        Path.GetExtension(category.ImageFile.FileName);
            await category.ImageFile.SaveAsync($"{_imageSavePath}/{image}", cancellationToken).ConfigureAwait(false);

            dbModel.Image = image;
        }

        _context.Categories.Update(dbModel);

        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;Category&gt; representing the asynchronous operation.</returns>
    public async ValueTask<Category?> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);

        if (dbModel is null)
        {
            return null;
        }

        dbModel.IsDeleted = true;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Categories.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
    public async ValueTask<bool> DeleteImageAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return false;
        }

        var path = $"{_imageSavePath}/{dbModel.Image}";

        DeleteFileIfExist(path);

        dbModel.Image = null;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Categories.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }

    /// <summary>
    /// Actives and in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;OneOf`2&gt; representing the asynchronous operation.</returns>
    public async Task<OneOf<bool, NotFound>> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return new NotFound();
        }

        dbModel.IsActive = !dbModel.IsActive;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Categories.Update(dbModel);

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return dbModel.IsActive;
    }
}