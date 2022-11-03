namespace Yogeshwar.Service.Service;

internal class ServiceService : IServiceService
{
    private readonly YogeshwarContext _context;

    public ServiceService(YogeshwarContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    async Task<DataTableResponseCarrier<ServiceDto>> IServiceService.GetByFilterAsync(DataTableFilterDto filterDto)
    {
        var result = _context.CustomerServices.AsNoTracking();

        if (!string.IsNullOrEmpty(filterDto.SearchValue))
        {
            result = result.Where(x => x.WorkerName.Contains(filterDto.SearchValue) ||
                                       x.Customer.FirstName.Contains(filterDto.SearchValue) ||
                                       x.Customer.LastName.Contains(filterDto.SearchValue));
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

        IList<ServiceDto> data = await result.Include(x => x.Customer)
            .Select(x => DtoSelector(x)).ToListAsync().ConfigureAwait(false);

        data = data.AsQueryable().OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder).ToArray();

        model.Data = data;

        return model;
    }

    private static ServiceDto DtoSelector(DB.Models.CustomerService service) => new()
    {
        Id = service.Id,
        CompletedDate = service.ServiceCompletedDate,
        CustomerId = service.CustomerId,
        Description = service.Description,
        WorkerName = service.WorkerName,
        ServiceStatus = service.Status,
        ServiceStatusString = ((ServiceStatus)service.Status).ToString(),
        CustomerName = service.Customer.FirstName + " " + service.Customer.LastName,
        ComplainDate = service.ComplainDate.ToString("dd-MM-yyyy"),
    };

    public async Task<int> CreateOrUpdateAsync(ServiceDto service)
    {
        if (service.Id < 1)
        {
            return await CreateAsync(service).ConfigureAwait(false);
        }

        return await UpdateAsync(service).ConfigureAwait(false);
    }

    private async ValueTask<int> CreateAsync(ServiceDto service)
    {
        var dbModel = new Yogeshwar.DB.Models.CustomerService
        {
            WorkerName = service.WorkerName,
            ComplainDate = DateTime.Now,
            CustomerId = service.CustomerId,
            Description = service.Description,
            ServiceCompletedDate = service.CompletedDate,
            Status = service.ServiceStatus
        };

        await _context.CustomerServices.AddAsync(dbModel).ConfigureAwait(false);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    private async ValueTask<int> UpdateAsync(ServiceDto service)
    {
        var dbModel = await _context.CustomerServices
            .FirstOrDefaultAsync(x => x.Id == service.Id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        dbModel.WorkerName = service.WorkerName;
        dbModel.CustomerId = service.CustomerId;
        dbModel.Description = service.Description;
        dbModel.ServiceCompletedDate = service.CompletedDate;
        dbModel.Status = service.ServiceStatus;

        _context.CustomerServices.Update(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<ServiceDto?> GetSingleAsync(int id)
    {
        return await _context.CustomerServices.AsNoTracking()
            .Where(x => x.Id == id).Include(x => x.Customer).Select(x => DtoSelector(x))
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<DB.Models.CustomerService?> DeleteAsync(int id)
    {
        var dbModel = await _context.CustomerServices
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return null;
        }

        _context.CustomerServices.Remove(dbModel);

        await _context.SaveChangesAsync().ConfigureAwait(false);

        return dbModel;
    }
}
