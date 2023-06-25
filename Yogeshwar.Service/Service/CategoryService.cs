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
    /// Get by identifier as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;CategoryDto&gt; representing the asynchronous operation.</returns>
    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _cachingService.GetCategoriesAsync(cancellationToken).ConfigureAwait(false);

        return data.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Create or update as an asynchronous operation.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;CategoryDto&gt; representing the asynchronous operation.</returns>
    public async Task<CategoryDto?> CreateOrUpdateAsync(CategoryDto category, CancellationToken cancellationToken)
    {
        if (category.Id < 1)
        {
            return await CreateAsync(category, cancellationToken).ConfigureAwait(false);
        }

        return await UpdateAsync(category, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Create as an asynchronous operation.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;CategoryDto&gt; representing the asynchronous operation.</returns>
    private async Task<CategoryDto> CreateAsync(CategoryDto category, CancellationToken cancellationToken)
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

        var dbModel = new Category
        {
            Name = category.Name,
            HsnNo = category.HsnNo,
            Image = image,
            IsActive = true,
            CreatedDate = DateTime.Now,
            CreatedBy = _currentUserService.GetCurrentUserId()
        };

        await _context.Categories.AddAsync(dbModel, cancellationToken).ConfigureAwait(false);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveCategories();

        return _mappingService.Map(dbModel);
    }

    /// <summary>
    /// Update as an asynchronous operation.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;CategoryDto&gt; representing the asynchronous operation.</returns>
    private async Task<CategoryDto?> UpdateAsync(CategoryDto category, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == category.Id && !x.IsDeleted, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return null;
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
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveCategories();

        return _mappingService.Map(dbModel);
    }

    /// <summary>
    /// Delete as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;CategoryDto&gt; representing the asynchronous operation.</returns>
    public async Task<CategoryDto?> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken)
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
    /// Delete image as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;CategoryDto&gt; representing the asynchronous operation.</returns>
    public async Task<CategoryDto?> DeleteImageAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return null;
        }

        DeleteFileIfExist(_rootPath + dbModel.Image);

        dbModel.Image = null;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Categories.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveCategories();

        return _mappingService.Map(dbModel);
    }

    /// <summary>
    /// Active in active record as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;CategoryDto&gt; representing the asynchronous operation.</returns>
    public async Task<CategoryDto?> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return null;
        }

        dbModel.IsActive = !dbModel.IsActive;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Categories.Update(dbModel);

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveCategories();

        return _mappingService.Map(dbModel);
    }
}