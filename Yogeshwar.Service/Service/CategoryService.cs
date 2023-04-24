namespace Yogeshwar.Service.Service;

/// <summary>
/// Class CategoryService.
/// Implements the <see cref="ICategoryService" />
/// </summary>
/// <seealso cref="ICategoryService" />
[RegisterService(ServiceLifetime.Scoped, typeof(ICategoryService))]
internal sealed class CategoryService : ICategoryService
{
    /// <summary>
    /// The context
    /// </summary>
    private readonly YogeshwarContext _context;

    /// <summary>
    /// The current user service
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

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
    /// The prefix path
    /// </summary>
    private const string PrefixPath = "/DataImages/Category/";

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryService" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="currentUserService">The current user service.</param>
    /// <param name="cachingService">The caching service.</param>
    /// <param name="mappingService">The mapping service.</param>
    public CategoryService(YogeshwarContext context, IWebHostEnvironment hostEnvironment,
        ICurrentUserService currentUserService, ICachingService cachingService,
        IMappingService mappingService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _cachingService = cachingService;
        _mappingService = mappingService;
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
    /// Gets the by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;DataTableResponseCarrier&lt;CategoryDto&gt;&gt;.</returns>
    async Task<DataTableResponseCarrier<CategoryDto>> ICategoryService.GetByFilterAsync(
        DataTableFilterDto filterDto, CancellationToken cancellationToken)
    {
        var cachedData = await _cachingService.GetCategoriesAsync(cancellationToken).ConfigureAwait(false);

        var result = cachedData.AsQueryable();

        if (!string.IsNullOrEmpty(filterDto.SearchValue))
        {
            result = result.Where(x => x.Name.Contains(filterDto.SearchValue));
        }

        var model = new DataTableResponseCarrier<CategoryDto>
        {
            TotalCount = result.Count()
        };

        result = result.Skip(filterDto.Skip);

        if (filterDto.Take != -1)
        {
            result = result.Take(filterDto.Take);
        }

        var data = result.OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder)
            .ToArray();

        model.Data = data;

        return model;
    }

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;CategoryDto&gt; representing the asynchronous operation.</returns>
    public async Task<CategoryDto?> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _cachingService.GetCategoriesAsync(cancellationToken).ConfigureAwait(false);

        return data.FirstOrDefault(x => x.Id == id);
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
            image = PrefixPath +
                    Guid.NewGuid().ToString().Replace("-", "") +
                    Path.GetExtension(category.ImageFile.FileName);

            await category.ImageFile.SaveAsync(_rootPath + image, cancellationToken)
                .ConfigureAwait(false);
        }

        var dbModel = _mappingService.Map(category);

        dbModel.Image = image;
        dbModel.IsActive = true;
        dbModel.CreatedDate = DateTime.Now;
        dbModel.CreatedBy = _currentUserService.GetCurrentUserId();

        await _context.Categories.AddAsync(dbModel, cancellationToken).ConfigureAwait(false);
        var count = await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveCategories();

        return count;
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
            .FirstOrDefaultAsync(x => x.Id == category.Id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return 0;
        }

        dbModel.Name = category.Name;
        dbModel.HsnNo = category.HsnNo;
        dbModel.ModifiedDate = DateTime.Now;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();

        if (category.ImageFile is not null)
        {
            var image = PrefixPath +
                Guid.NewGuid().ToString().Replace("-", "") +
                Path.GetExtension(category.ImageFile.FileName);

            await category.ImageFile.SaveAsync(_rootPath + image, cancellationToken)
                .ConfigureAwait(false);

            DeleteFileIfExist(_rootPath + dbModel.Image);

            dbModel.Image = image;
        }

        _context.Categories.Update(dbModel);
        var count = await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveCategories();

        return count;
    }

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;Category&gt; representing the asynchronous operation.</returns>
    async ValueTask<Category?> ICategoryService.DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return null;
        }

        dbModel.IsDeleted = true;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Categories.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveCategories();

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

        DeleteFileIfExist(_rootPath + dbModel.Image);

        dbModel.Image = null;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Categories.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveCategories();

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
        var dbModel = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
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

        _cachingService.RemoveCategories();

        return dbModel.IsActive;
    }
}