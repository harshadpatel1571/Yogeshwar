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
    /// The mapping service
    /// </summary>
    private readonly IMappingService _mappingService;

    /// <summary>
    /// The current user service
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// The root path
    /// </summary>
    private readonly string _rootPath;

    /// <summary>
    /// The prefix path
    /// </summary>
    private const string PrefixPath = "/DataImages/Customer/";

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="currentUserService">The current user service.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="mappingService">The mapping service.</param>
    public CustomerService(YogeshwarContext context, ICurrentUserService currentUserService,
        IWebHostEnvironment hostEnvironment, IMappingService mappingService)
    {
        _currentUserService = currentUserService;
        _mappingService = mappingService;
        _context = context;
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
    /// <returns>Task&lt;DataTableResponseCarrier&lt;CustomerDto&gt;&gt;.</returns>
    async Task<DataTableResponseCarrier<CustomerDto>> ICustomerService.GetByFilterAsync(
        DataTableFilterDto filterDto, CancellationToken cancellationToken)
    {
        var result = _context.Customers.Where(x => !x.IsDeleted)
            .Include(x => x.CustomerAddresses)
            .AsNoTracking();

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
            .Select(x => _mappingService.Map(x))
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        model.Data = data;

        return model;
    }

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;CustomerDto&gt; representing the asynchronous operation.</returns>
    public async Task<CustomerDto?> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Customers.AsNoTracking()
            .Where(x => x.Id == id && !x.IsDeleted)
            .Include(x => x.CustomerAddresses)
            .Select(x => _mappingService.Map(x))
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    public async Task<int> UpsertAsync(CustomerDto customer, CancellationToken cancellationToken)
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
    /// <param name="customerDto">The customer.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    private async ValueTask<int> CreateAsync(CustomerDto customerDto, CancellationToken cancellationToken)
    {
        var image = (string?)null;

        if (customerDto.ImageFile is not null)
        {
            image = PrefixPath +
                    Guid.NewGuid().ToString().Replace("-", "") +
                    Path.GetExtension(customerDto.ImageFile.FileName);

            await customerDto.ImageFile.SaveAsync(_rootPath + image, cancellationToken)
                .ConfigureAwait(false);
        }

        var dbModel = _mappingService.Map(customerDto);

        dbModel.Image = image;
        dbModel.IsActive = true;
        dbModel.CreatedBy = _currentUserService.GetCurrentUserId();
        dbModel.CreatedDate = DateTime.Now;

        await _context.Customers.AddAsync(dbModel, cancellationToken).ConfigureAwait(false);

        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates the asynchronous.
    /// </summary>
    /// <param name="customerDto">The customer.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
    private async ValueTask<int> UpdateAsync(CustomerDto customerDto, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == customerDto.Id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        if (customerDto.ImageFile is not null)
        {
            var image = Guid.NewGuid().ToString().Replace("-", "") +
                        Path.GetExtension(customerDto.ImageFile.FileName);

            await customerDto.ImageFile.SaveAsync(_rootPath + image, cancellationToken)
                .ConfigureAwait(false);

            DeleteFileIfExist(_rootPath + dbModel.Image);

            dbModel.Image = image;
        }

        dbModel.FirstName = customerDto.FirstName;
        dbModel.LastName = customerDto.LastName;
        dbModel.Email = customerDto.Email;
        dbModel.PhoneNo = customerDto.PhoneNo;
        dbModel.AccountHolderName = customerDto.AccountHolderName;
        dbModel.AccountNumber = customerDto.AccountNumber;
        dbModel.BankName = customerDto.BankName;
        dbModel.BranchName = customerDto.BranchName;
        dbModel.GstNumber = customerDto.GstNumber;
        dbModel.IfscCode = customerDto.IfscCode;
        dbModel.ModifiedBy = _currentUserService.GetCurrentUserId();
        dbModel.ModifiedDate = DateTime.Now;

        await _context.CustomerAddresses
            .Where(x => x.CustomerId == customerDto.Id)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);

        var newAddresses = customerDto.CustomerAddresses.Select(_mappingService.Map);

        _context.CustomerAddresses.AddRange(newAddresses);

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

        DeleteFileIfExist(_rootPath + dbModel.Image);

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