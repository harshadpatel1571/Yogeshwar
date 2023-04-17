namespace Yogeshwar.Service.Service;

/// <summary>
/// Class AccessoriesService.
/// Implements the <see cref="IAccessoriesService" />
/// </summary>
/// <seealso cref="IAccessoriesService" />
[RegisterService(ServiceLifetime.Scoped, typeof(IAccessoriesService))]
internal sealed class AccessoriesService : IAccessoriesService
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
    private const string PrefixPath = "/DataImages/Accessories/";

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessoriesService" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="currentUserService">The current user service.</param>
    /// <param name="cachingService">The caching service.</param>
    /// <param name="mappingService">The mapping service.</param>
    public AccessoriesService(YogeshwarContext context, IWebHostEnvironment hostEnvironment,
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
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;DataTableResponseCarrier&lt;AccessoriesDto&gt;&gt;.</returns>
    async Task<DataTableResponseCarrier<AccessoriesDto>> IAccessoriesService.GetByFilterAsync(
        DataTableFilterDto filterDto, CancellationToken cancellationToken)
    {
        var cachedData = await _cachingService.GetAccessoriesAsync(cancellationToken).ConfigureAwait(false);

        var result = cachedData.AsQueryable();

        if (!string.IsNullOrEmpty(filterDto.SearchValue))
        {
            result = result.Where(x => x.Name.Contains(filterDto.SearchValue));
        }

        var model = new DataTableResponseCarrier<AccessoriesDto>
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
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;AccessoriesDto&gt; representing the asynchronous operation.</returns>
    public async Task<AccessoriesDto?> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _cachingService.GetAccessoriesAsync(cancellationToken).ConfigureAwait(false);

        return data.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="accessoriesDto">The customer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    public async Task<int> CreateOrUpdateAsync(AccessoriesDto accessoriesDto, CancellationToken cancellationToken)
    {
        if (accessoriesDto.Id < 1)
        {
            return await CreateAsync(accessoriesDto, cancellationToken).ConfigureAwait(false);
        }

        return await UpdateAsync(accessoriesDto, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="accessory">The accessory.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    private async ValueTask<int> CreateAsync(AccessoriesDto accessory, CancellationToken cancellationToken)
    {
        var image = (string?)null;

        if (accessory.File is not null)
        {
            image = PrefixPath +
                    Guid.NewGuid().ToString().Replace("-", "") +
                    Path.GetExtension(accessory.File.FileName);

            await accessory.File.SaveAsync(_rootPath + image, cancellationToken)
                .ConfigureAwait(false);
        }

        var dbModel = _mappingService.Map(accessory);

        dbModel.Image = image;
        dbModel.IsActive = true;
        dbModel.CreatedDate = DateTime.Now;
        dbModel.CreatedBy = _currentUserService.GetCurrentUserId();

        await _context.Accessories.AddAsync(dbModel, cancellationToken).ConfigureAwait(false);
        var count = await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        accessory.Id = dbModel.Id;

        _cachingService.RemoveAccessories();

        return count;
    }

    /// <summary>
    /// Updates the asynchronous.
    /// </summary>
    /// <param name="accessory">The accessory.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    private async ValueTask<int> UpdateAsync(AccessoriesDto accessory, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Accessories
            .FirstOrDefaultAsync(x => x.Id == accessory.Id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        if (accessory.File is not null)
        {
            var image = PrefixPath +
                        Guid.NewGuid().ToString().Replace("-", "") +
                        Path.GetExtension(accessory.File.FileName);

            await accessory.File.SaveAsync(_rootPath + image, cancellationToken).ConfigureAwait(false);

            DeleteFileIfExist(_rootPath + dbModel.Image);

            dbModel.Image = PrefixPath + image;
        }

        dbModel.Name = accessory.Name;
        dbModel.MeasurementType = accessory.MeasurementType;
        dbModel.Description = accessory.Description;
        dbModel.Quantity = accessory.Quantity;
        dbModel.ModifiedDate = DateTime.Now;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();

        _context.Accessories.Update(dbModel);
        var count = await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveAccessories();

        return count;
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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    async Task<int> IAccessoriesService.DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Accessories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        dbModel.IsDeleted = true;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Accessories.Update(dbModel);
        var count = await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveAccessories();

        return count;
    }

    /// <summary>
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
    public async ValueTask<bool> DeleteImageAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Accessories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return false;
        }

        DeleteFileIfExist(_rootPath + dbModel.Image);

        dbModel.Image = null;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Accessories.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveAccessories();

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
        var dbModel = await _context.Accessories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return new NotFound();
        }

        dbModel.IsActive = !dbModel.IsActive;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Accessories.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveAccessories();

        return dbModel.IsActive;
    }
}