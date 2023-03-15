namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(ICustomerService))]
internal class CustomerService : ICustomerService
{
    private readonly YogeshwarContext _context;
    private readonly Lazy<ICurrentUserService> _currentUserService;


    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="currentUserService">The current user service.</param>
    public CustomerService(YogeshwarContext context, Lazy<ICurrentUserService> currentUserService)
    {
        _currentUserService = currentUserService;
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
    async Task<DataTableResponseCarrier<CustomerDto>> ICustomerService.GetByFilterAsync(DataTableFilterDto filterDto)
    {
        var result = _context.Customers.Where(x => !x.IsDeleted).AsNoTracking();

        if (!string.IsNullOrEmpty(filterDto.SearchValue))
        {
            result = result.Where(x => x.Email.Contains(filterDto.SearchValue) ||
                                       x.FirstName.Contains(filterDto.SearchValue) ||
                                       x.LastName.Contains(filterDto.SearchValue));
        }

        var model = new DataTableResponseCarrier<CustomerDto>
        {
            TotalCount = result.Count()
        };

        result = result.Skip(filterDto.Skip);

        if (filterDto.Take != -1)
        {
            result = result.Take(filterDto.Take);
        }

        var data = await result
            .OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder)
            .Select(x => DtoSelector(x))
            .ToListAsync().ConfigureAwait(false);

        model.Data = data;

        return model;
    }

    /// <summary>
    /// Select the Dto.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns></returns>
    private static CustomerDto DtoSelector(Customer customer) =>
        new()
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Address = customer.Address,
            City = customer.City,
            PhoneNo = customer.PhoneNo,
            PinCode = customer.Pincode,
            IsActive = customer.IsActive,
            CreatedBy = customer.CreatedBy,
            CreatedDate = customer.CreatedDate,
            ModifiedBy = customer.ModifiedBy,
            ModifiedDate = customer.ModifiedDate
        };

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async Task<CustomerDto?> GetSingleAsync(int id)
    {
        return await _context.Customers.AsNoTracking()
            .Where(x => x.Id == id).Select(x => DtoSelector(x))
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns></returns>
    public async Task<int> CreateOrUpdateAsync(CustomerDto customer)
    {
        if (customer.Id < 1)
        {
            return await CreateAsync(customer).ConfigureAwait(false);
        }

        return await UpdateAsync(customer).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns></returns>
    private async ValueTask<int> CreateAsync(CustomerDto customer)
    {
        var dbModel = new Customer
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNo = customer.PhoneNo,
            Address = customer.Address,
            IsActive = true,
            City = customer.City,
            Pincode = customer.PinCode,
            CreatedBy = _currentUserService.Value.GetCurrentUserId(),
            CreatedDate = DateTime.Now
        };

        await _context.Customers.AddAsync(dbModel).ConfigureAwait(false);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Updates the asynchronous.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns></returns>
    private async ValueTask<int> UpdateAsync(CustomerDto customer)
    {
        var dbModel = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == customer.Id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        dbModel.FirstName = customer.FirstName;
        dbModel.LastName = customer.LastName;
        dbModel.Email = customer.Email;
        dbModel.PhoneNo = customer.PhoneNo;
        dbModel.Address = customer.Address;
        dbModel.City = customer.City;
        dbModel.Pincode = customer.PinCode;
        dbModel.ModifiedBy = _currentUserService.Value.GetCurrentUserId();
        dbModel.IsActive = customer.IsActive;
        dbModel.ModifiedDate = DateTime.Now;

        _context.Customers.Update(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(int id)
    {
        var dbModel = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        dbModel.IsDeleted = true;
        dbModel.ModifiedBy = _currentUserService.Value.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Customers.Update(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}