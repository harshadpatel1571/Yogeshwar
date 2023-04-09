namespace Yogeshwar.Service.Service;

/// <summary>
/// Class AccessoriesService.
/// Implements the <see cref="IAccessoriesService" />
/// </summary>
/// <seealso cref="IAccessoriesService" />
[RegisterService(ServiceLifetime.Scoped, typeof(IAccessoriesService))]
internal class AccessoriesService : IAccessoriesService
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
    /// The save path
    /// </summary>
    private readonly string _savePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessoriesService" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="currentUserService">The current user service.</param>
    public AccessoriesService(YogeshwarContext context, IConfiguration configuration,
        IWebHostEnvironment hostEnvironment, ICurrentUserService currentUserService)
    {
        _context = context;
        _configuration = configuration;
        _currentUserService = currentUserService;
        _savePath = $"{hostEnvironment.WebRootPath}/DataImages/Accessories";
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
        var result = _context.Accessories.Where(x => !x.IsDeleted).AsNoTracking();

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

        var data = await result
            .OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder)
            .Select(x => DtoSelector(x, _configuration))
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        model.Data = data;

        return model;
    }

    /// <summary>
    /// Select the Dto.
    /// </summary>
    /// <param name="accessory">The accessory.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>AccessoriesDto.</returns>
    private static AccessoriesDto DtoSelector(Accessory accessory, IConfiguration configuration) =>
        new()
        {
            Id = accessory.Id,
            Name = accessory.Name,
            Description = accessory.Description,
            Image = accessory.Image == null ? null : $"{configuration["File:ReadPath"]}/Accessories/{accessory.Image}",
            Quantity = accessory.Quantity,
            IsActive = accessory.IsActive,
            CreatedDate = accessory.CreatedDate,
            ModifiedDate = accessory.ModifiedDate,
        };

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;AccessoriesDto&gt; representing the asynchronous operation.</returns>
    public async Task<AccessoriesDto?> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Accessories.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => DtoSelector(x, _configuration))
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    public async Task<int> CreateOrUpdateAsync(AccessoriesDto customer, CancellationToken cancellationToken)
    {
        if (customer.Id < 1)
        {
            return await CreateAsync(customer, cancellationToken).ConfigureAwait(false);
        }

        return await UpdateAsync(customer, cancellationToken).ConfigureAwait(false);
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
            image = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                    Path.GetExtension(accessory.File.FileName);
            await accessory.File.SaveAsync($"{_savePath}/{image}", cancellationToken).ConfigureAwait(false);
        }

        var dbModel = new Accessory
        {
            Id = accessory.Id,
            Name = accessory.Name,
            Description = accessory.Description,
            Image = image,
            Quantity = accessory.Quantity,
            IsActive = true,
            CreatedDate = DateTime.Now,
            CreatedBy = _currentUserService.GetCurrentUserId(),
        };

        await _context.Accessories.AddAsync(dbModel, cancellationToken).ConfigureAwait(false);

        var count = await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        accessory.Id = dbModel.Id;

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
            .FirstOrDefaultAsync(x => x.Id == accessory.Id, cancellationToken).ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        if (accessory.File is not null)
        {
            var image = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                        Path.GetExtension(accessory.File.FileName);
            await accessory.File.SaveAsync($"{_savePath}/{image}", cancellationToken).ConfigureAwait(false);

            DeleteFileIfExist($"{_savePath}/{dbModel.Image}");

            dbModel.Image = image;
        }

        dbModel.Id = accessory.Id;
        dbModel.Name = accessory.Name;
        dbModel.Description = accessory.Description;
        dbModel.Quantity = accessory.Quantity;
        dbModel.ModifiedDate = DateTime.Now;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();

        _context.Accessories.Update(dbModel);

        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
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
    public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Accessories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        dbModel.IsDeleted = true;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Accessories.Update(dbModel);

        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
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
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);

        if (dbModel == null)
        {
            return false;
        }

        var path = $"{_savePath}/{dbModel.Image}";

        DeleteFileIfExist(path);

        dbModel.Image = null;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

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
        var dbModel = await _context.Accessories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
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

        return dbModel.IsActive;
    }
}