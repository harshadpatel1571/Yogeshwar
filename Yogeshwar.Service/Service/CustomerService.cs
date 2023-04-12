namespace Yogeshwar.Service.Service;

/// <summary>
/// Class CustomerService.
/// Implements the <see cref="ICustomerService" />
/// </summary>
/// <seealso cref="ICustomerService" />
[RegisterService(ServiceLifetime.Scoped, typeof(ICustomerService))]
internal sealed class CustomerService : ICustomerService
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
    /// Initializes a new instance of the <see cref="CustomerService" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="currentUserService">The current user service.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="hostEnvironment">The host environment.</param>
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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;DataTableResponseCarrier&lt;CustomerDto&gt;&gt;.</returns>
    async Task<DataTableResponseCarrier<CustomerDto>> ICustomerService.GetByFilterAsync(
        DataTableFilterDto filterDto, CancellationToken cancellationToken)
    {
        var result = _context.Customers.Where(x => !x.IsDeleted).AsNoTracking();

        if (!string.IsNullOrEmpty(filterDto.SearchValue))
        {
            result = result.Where(x => x.FirstName.Contains(filterDto.SearchValue) ||
                                       x.LastName.Contains(filterDto.SearchValue) ||
                                       x.GstNumber.Contains(filterDto.SearchValue));
        }

        var model = new DataTableResponseCarrier<CustomerDto>
        {
            TotalCount = await result.CountAsync(cancellationToken).ConfigureAwait(false)
        };

        result = result.OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder);

        result = result.Skip(filterDto.Skip);

        if (filterDto.Take != -1)
        {
            result = result.Take(filterDto.Take);
        }

        var data = await result
            .Select(x => DtoSelector(x, _configuration))
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        model.Data = data;

        return model;
    }

    /// <summary>
    /// Select the Dto.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>CustomerDto.</returns>
    private static CustomerDto DtoSelector(Customer customer, IConfiguration configuration) =>
        new()
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            //Address = customer.Address,
            //City = customer.City,
            PhoneNo = customer.PhoneNo,
            //PinCode = customer.PinCode,
            AccountHolderName = customer.AccountHolderName,
            AccountNumber = customer.AccountNumber,
            BankName = customer.BankName,
            BranchName = customer.BranchName,
            GstNumber = customer.GstNumber,
            IFSCCode = customer.IfscCode,
            Image = customer.Image == null ? null : $"{configuration["File:ReadPath"]}/Customer/{customer.Image}",
            IsActive = customer.IsActive,
            CreatedDate = customer.CreatedDate,
            ModifiedDate = customer.ModifiedDate
        };

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;CustomerDto&gt; representing the asynchronous operation.</returns>
    public async Task<CustomerDto?> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Customers.AsNoTracking()
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
    public async Task<int> CreateOrUpdateAsync(CustomerDto customer, CancellationToken cancellationToken)
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
    /// <param name="customer">The customer.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    private async ValueTask<int> CreateAsync(CustomerDto customer, CancellationToken cancellationToken)
    {
        var image = (string?)null;

        if (customer.ImageFile is not null)
        {
            image = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                    Path.GetExtension(customer.ImageFile.FileName);
            await customer.ImageFile.SaveAsync($"{_savePath}/{image}", cancellationToken).ConfigureAwait(false);
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
            GstNumber = customer.GstNumber,
            IfscCode = customer.IFSCCode,
            //Address = customer.Address,
            Image = image,
            IsActive = true,
            //City = customer.City,
            //PinCode = customer.PinCode,
            CreatedBy = _currentUserService.GetCurrentUserId(),
            CreatedDate = DateTime.Now
        };

        await _context.Customers.AddAsync(dbModel, cancellationToken).ConfigureAwait(false);

        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates the asynchronous.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    private async ValueTask<int> UpdateAsync(CustomerDto customer, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == customer.Id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        if (customer.ImageFile is not null)
        {
            var image = string.Join(null, Guid.NewGuid().ToString().Split('-')) +
                        Path.GetExtension(customer.ImageFile.FileName);
            await customer.ImageFile.SaveAsync($"{_savePath}/{image}", cancellationToken)
                .ConfigureAwait(false);

            DeleteFileIfExist($"{_savePath}/{dbModel.Image}");

            dbModel.Image = image;
        }

        dbModel.FirstName = customer.FirstName;
        dbModel.LastName = customer.LastName;
        dbModel.Email = customer.Email;
        dbModel.PhoneNo = customer.PhoneNo;
        //dbModel.Address = customer.Address;
        dbModel.AccountHolderName = customer.AccountHolderName;
        dbModel.AccountNumber = customer.AccountNumber;
        dbModel.BankName = customer.BankName;
        dbModel.BranchName = customer.BranchName;
        dbModel.GstNumber = customer.GstNumber;
        dbModel.IfscCode = customer.IFSCCode;
        //dbModel.City = customer.City;
        //dbModel.PinCode = customer.PinCode;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Customers.Update(dbModel);

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
        var dbModel = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        dbModel.IsDeleted = true;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Customers.Update(dbModel);

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
        var dbModel = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return false;
        }

        var path = $"{_savePath}/{dbModel.Image}";

        DeleteFileIfExist(path);

        dbModel.Image = null;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Customers.Update(dbModel);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }

    /// <summary>
    /// Actives and in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;OneOf`2&gt; representing the asynchronous operation.</returns>
    public async Task<OneOf<bool, NotFound>> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return new NotFound();
        }

        dbModel.IsActive = !dbModel.IsActive;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        _context.Customers.Update(dbModel);

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return dbModel.IsActive;
    }
}