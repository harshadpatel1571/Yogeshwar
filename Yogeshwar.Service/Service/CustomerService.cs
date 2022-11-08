namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(ICustomerService))]
internal class CustomerService : ICustomerService
{
    private readonly YogeshwarContext _context;

    public CustomerService(YogeshwarContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    async Task<DataTableResponseCarrier<CustomerDto>> ICustomerService.GetByFilterAsync(DataTableFilterDto filterDto)
    {
        var result = _context.Customers.AsNoTracking();

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

        var data = await result.OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder)
            .Select(x => DtoSelector(x)).ToListAsync().ConfigureAwait(false);

        model.Data = data;

        return model;
    }

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
            PinCode = customer.Pincode
        };

    public async Task<CustomerDto?> GetSingleAsync(int id)
    {
        return await _context.Customers.AsNoTracking()
            .Where(x => x.Id == id).Select(x => DtoSelector(x))
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<int> CreateOrUpdateAsync(CustomerDto customer)
    {
        if (customer.Id < 1)
        {
            return await CreateAsync(customer).ConfigureAwait(false);
        }

        return await UpdateAsync(customer).ConfigureAwait(false);
    }

    private async ValueTask<int> CreateAsync(CustomerDto customer)
    {
        var dbModel = new Customer
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNo = customer.PhoneNo,
            Address = customer.Address,
            City = customer.City,
            Pincode = customer.PinCode,
            CreatedBy = customer.CreatedBy
        };

        await _context.Customers.AddAsync(dbModel).ConfigureAwait(false);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

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
        dbModel.Pincode = customer.PinCode;
        dbModel.ModifiedBy = customer.ModifiedBy;
        dbModel.ModifiedDate = DateTime.Now;

        _context.Customers.Update(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<Customer?> DeleteAsync(int id)
    {
        var dbModel = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return null;
        }

        _context.Customers.Remove(dbModel);

        await _context.SaveChangesAsync().ConfigureAwait(false);

        return dbModel;
    }
}