namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(IServiceService))]
internal class ServiceService : IServiceService
{
    private readonly YogeshwarContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public ServiceService(YogeshwarContext context)
    {
        _context = context;
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
    async Task<DataTableResponseCarrier<ServiceDto>> IServiceService.GetByFilterAsync(DataTableFilterDto filterDto)
    {
        var result = _context.CustomerServices.Where(x => !x.IsDeleted).AsNoTracking();

        if (!string.IsNullOrEmpty(filterDto.SearchValue))
        {
            result = result.Where(x => x.WorkerName.Contains(filterDto.SearchValue) ||
                                       ("Order #" + x.OrderId + " - " + x.Order.Customer.FirstName + " " +
                                        x.Order.Customer.LastName).Contains(filterDto.SearchValue));
        }

        var model = new DataTableResponseCarrier<ServiceDto>
        {
            TotalCount = result.Count()
        };

        result = result.Skip(filterDto.Skip);

        if (filterDto.Take != -1)
        {
            result = result.Take(filterDto.Take);
        }

        IList<ServiceDto> data = await result
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .Select(x => DtoSelector(x))
            .ToListAsync().ConfigureAwait(false);

        data = data.AsQueryable().OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder).ToArray();

        model.Data = data;

        return model;
    }

    /// <summary>
    /// Select the Dto.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <returns></returns>
    private static ServiceDto DtoSelector(DB.Models.CustomerService service) => new()
    {
        Id = service.Id,
        CompletedDate = service.ServiceCompletionDate,
        OrderId = service.Order.CustomerId,
        Description = service.Description,
        WorkerName = service.WorkerName,
        ServiceStatus = service.Status,
        ServiceStatusString = ((ServiceStatus)service.Status).ToString(),
        OrderCustomerName = "Order #" + service.OrderId + " - " +
                            service.Order.Customer.FirstName + " " + service.Order.Customer.LastName,
        ComplainDate = service.ComplainDate.ToString("dd-MM-yyyy"),
    };

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <returns></returns>
    public async Task<int> CreateOrUpdateAsync(ServiceDto service)
    {
        if (service.Id < 1)
        {
            return await CreateAsync(service).ConfigureAwait(false);
        }

        return await UpdateAsync(service).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <returns></returns>
    private async ValueTask<int> CreateAsync(ServiceDto service)
    {
        var dbModel = new Yogeshwar.DB.Models.CustomerService
        {
            WorkerName = service.WorkerName,
            ComplainDate = DateTime.Now,
            OrderId = service.OrderId,
            Description = service.Description,
            ServiceCompletionDate = service.CompletedDate,
            Status = service.ServiceStatus
        };

        await _context.CustomerServices.AddAsync(dbModel).ConfigureAwait(false);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Updates the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <returns></returns>
    private async ValueTask<int> UpdateAsync(ServiceDto service)
    {
        var dbModel = await _context.CustomerServices
            .FirstOrDefaultAsync(x => x.Id == service.Id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        dbModel.WorkerName = service.WorkerName;
        dbModel.OrderId = service.OrderId;
        dbModel.Description = service.Description;
        dbModel.ServiceCompletionDate = service.CompletedDate;
        dbModel.Status = service.ServiceStatus;

        _context.CustomerServices.Update(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async Task<ServiceDto?> GetSingleAsync(int id)
    {
        return await _context.CustomerServices.AsNoTracking()
            .Where(x => x.Id == id).Include(x => x.Order)
            .ThenInclude(x => x.Customer).Select(x => DtoSelector(x))
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(int id)
    {
        var dbModel = await _context.CustomerServices
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        _context.CustomerServices.Remove(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}