namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(ICustomerService))]
internal class CustomerService : ICustomerService
{
    private readonly YogeshwarContext _context;
    private readonly IConfiguration _configuration;
    private readonly ICurrentUserService _currentUserService;
    private readonly string _savePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerService" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="currentUserService">The current user service.</param>
    /// <param name="configuration">The configuration.</param>
    public CustomerService(YogeshwarContext context, ICurrentUserService currentUserService,
        IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        _currentUserService = currentUserService;
        _context = context;
        _configuration = configuration;
        _savePath = $"{hostEnvironment.WebRootPath}/DataImages/Customer";
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
            result = result.Where(x => x.FirstName.Contains(filterDto.SearchValue) ||
                                       x.LastName.Contains(filterDto.SearchValue) ||
                                       x.Gstnumber.Contains(filterDto.SearchValue));
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
            .Select(x => DtoSelector(x, _configuration))
            .ToListAsync().ConfigureAwait(false);

        model.Data = data;

        return model;
    }

    /// <summary>
    /// Select the Dto.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns></returns>
    private static CustomerDto DtoSelector(Customer customer, IConfiguration configuration) =>
        new()
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Address = customer.Address,
            City = customer.City,
            PhoneNo = customer.PhoneNo,
            PinCode = customer.PinCode,
            AccountHolderName = customer.AccountHolderName,
            AccountNumber = customer.AccountNumber,
            BankName = customer.BankName,
            BranchName = customer.BranchName,
            GstNumber = customer.Gstnumber,
            IFSCCode = customer.Ifsccode,
            Image = customer.Image == null ? null : $"{configuration["File:ReadPath"]}/Customer/{customer.Image}",
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
            .Where(x => x.Id == id).Select(x => DtoSelector(x, _configuration))
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
        var image = (string?)null;

        if (customer.ImageFile is not null)
        {
            image = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                    Path.GetExtension(customer.ImageFile.FileName);
            await customer.ImageFile.SaveAsync($"{_savePath}/{image}").ConfigureAwait(false);
        }

        var dbModel = new Customer
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNo = customer.PhoneNo,
            AccountHolderName = customer.AccountHolderName,
            AccountNumber = customer.AccountNumber,
            BankName = customer.BankName,
            BranchName = customer.BranchName,
            Gstnumber = customer.GstNumber,
            Ifsccode = customer.IFSCCode,
            Address = customer.Address,
            Image = image,
            IsActive = true,
            City = customer.City,
            PinCode = customer.PinCode,
            CreatedBy = _currentUserService.GetCurrentUserId(),
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

        if (customer.ImageFile is not null)
        {
            var image = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                        Path.GetExtension(customer.ImageFile.FileName);
            await customer.ImageFile.SaveAsync($"{_savePath}/{image}").ConfigureAwait(false);

            DeleteFileIfExist($"{_savePath}/{dbModel.Image}");

            dbModel.Image = image;
        }

        dbModel.FirstName = customer.FirstName;
        dbModel.LastName = customer.LastName;
        dbModel.Email = customer.Email;
        dbModel.PhoneNo = customer.PhoneNo;
        dbModel.Address = customer.Address;
        dbModel.AccountHolderName = customer.AccountHolderName;
        dbModel.AccountNumber = customer.AccountNumber;
        dbModel.BankName = customer.BankName;
        dbModel.BranchName = customer.BranchName;
        dbModel.Gstnumber = customer.GstNumber;
        dbModel.Ifsccode = customer.IFSCCode;
        dbModel.City = customer.City;
        dbModel.PinCode = customer.PinCode;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.IsActive = customer.IsActive;
        dbModel.ModifiedDate = DateTime.Now;

        _context.Customers.Update(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
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
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Customers.Update(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }
    
    /// <summary>
    /// Actives and in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public async Task<OneOf<bool, NotFound>> ActiveInActiveRecordAsync(int id)
    {
        var dbModel = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return new NotFound();
        }

        dbModel.IsActive = !dbModel.IsActive;

        _context.Customers.Update(dbModel);

        await _context.SaveChangesAsync();

        return dbModel.IsActive;
    }
}