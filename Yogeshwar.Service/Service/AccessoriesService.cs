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
    /// Initializes a new instance of the <see cref="AccessoriesService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="currentUserService">The current user service.</param>
    /// <param name="cachingService">The caching service.</param>
    /// <param name="mappingService">The mapping service.</param>
    public AccessoriesService(YogeshwarContext context, IWebHostEnvironment hostEnvironment,
        ICurrentUserService currentUserService, ICachingService cachingService, IMappingService mappingService)
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
    public async Task<DataTableResponseCarrier<AccessoriesDto>> GetByFilterAsync(DataTableFilterDto filterDto,
        CancellationToken cancellationToken)
    {
        var cachedData = await _cachingService.GetAccessoriesAsync(cancellationToken)
            .ConfigureAwait(false);

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
    /// Get by ids as an asynchronous operation.
    /// </summary>
    /// <param name="ids">The ids.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;IList`1&gt; representing the asynchronous operation.</returns>
    public async Task<IList<AccessoriesDto>> GetByIdsAsync(IList<int> ids,
        CancellationToken cancellationToken)
    {
        var data = await _cachingService.GetAccessoriesAsync(cancellationToken)
            .ConfigureAwait(false);

        return data.Where(x => ids.Contains(x.Id)).ToArray();
    }

    /// <summary>
    /// Get by identifier as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;AccessoriesDto&gt; representing the asynchronous operation.</returns>
    public async Task<AccessoriesDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _cachingService.GetAccessoriesAsync(cancellationToken)
            .ConfigureAwait(false);

        return data.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="accessoriesDto">The customer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    public async Task<AccessoriesDto?> CreateOrUpdateAsync(AccessoriesDto accessoriesDto,
        CancellationToken cancellationToken)
    {
        if (accessoriesDto.Id < 1)
        {
            return await CreateAsync(accessoriesDto, cancellationToken).ConfigureAwait(false);
        }

        return await UpdateAsync(accessoriesDto, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Create as an asynchronous operation.
    /// </summary>
    /// <param name="accessory">The accessory.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;AccessoriesDto&gt; representing the asynchronous operation.</returns>
    private async Task<AccessoriesDto> CreateAsync(AccessoriesDto accessory, CancellationToken cancellationToken)
    {
        var image = (string?)null;

        if (accessory.ImageFile is not null)
        {
            image = PrefixPath +
                    Guid.NewGuid().ToString().Replace("-", "") +
                    Path.GetExtension(accessory.ImageFile.FileName);

            await accessory.ImageFile.SaveAsync(_rootPath + image, cancellationToken)
                .ConfigureAwait(false);
        }

        var dbModel = new Accessory
        {
            Name = accessory.Name,
            Description = accessory.Description,
            Quantity = accessory.Quantity,
            MeasurementType = accessory.MeasurementType,
            Price = accessory.Price,
            Image = image,
            IsActive = true,
            CreatedDate = DateTime.Now,
            CreatedBy = _currentUserService.GetCurrentUserId()
        };

        await _context.Accessories.AddAsync(dbModel, cancellationToken).ConfigureAwait(false);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveAccessories();

        return _mappingService.Map(dbModel);
    }

    /// <summary>
    /// Update as an asynchronous operation.
    /// </summary>
    /// <param name="accessory">The accessory.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;AccessoriesDto&gt; representing the asynchronous operation.</returns>
    private async ValueTask<AccessoriesDto?> UpdateAsync(AccessoriesDto accessory, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Accessories
            .FirstOrDefaultAsync(x => x.Id == accessory.Id && !x.IsDeleted, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return null;
        }

        if (accessory.ImageFile is not null)
        {
            var image = PrefixPath +
                        Guid.NewGuid().ToString().Replace("-", "") +
                        Path.GetExtension(accessory.ImageFile.FileName);

            await accessory.ImageFile.SaveAsync(_rootPath + image, cancellationToken).ConfigureAwait(false);

            DeleteFileIfExist(_rootPath + dbModel.Image);

            dbModel.Image = image;
        }

        dbModel.Name = accessory.Name;
        dbModel.MeasurementType = accessory.MeasurementType;
        dbModel.Description = accessory.Description;
        dbModel.Quantity = accessory.Quantity;
        dbModel.ModifiedDate = DateTime.Now;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();

        _context.Accessories.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveAccessories();

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
    /// Delete as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;AccessoriesDto&gt; representing the asynchronous operation.</returns>
    public async Task<AccessoriesDto?> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Accessories
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return null;
        }

        dbModel.IsDeleted = true;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Accessories.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveAccessories();

        return _mappingService.Map(dbModel);
    }

    /// <summary>
    /// Delete image as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;AccessoriesDto&gt; representing the asynchronous operation.</returns>
    public async Task<AccessoriesDto?> DeleteImageAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Accessories
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return null;
        }

        DeleteFileIfExist(_rootPath + dbModel.Image);

        dbModel.Image = null;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Accessories.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveAccessories();

        return _mappingService.Map(dbModel);
    }

    /// <summary>
    /// Active in active record as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;AccessoriesDto&gt; representing the asynchronous operation.</returns>
    public async Task<AccessoriesDto?> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Accessories
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return null;
        }

        dbModel.IsActive = !dbModel.IsActive;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Accessories.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _cachingService.RemoveAccessories();

        return _mappingService.Map(dbModel);
    }
}