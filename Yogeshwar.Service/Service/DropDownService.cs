namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(IDropDownService))]
internal class DropDownService : IDropDownService
{
    private readonly YogeshwarContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="DropDownService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public DropDownService(YogeshwarContext context)
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
    /// Binds the drop down for accessories asynchronous.
    /// </summary>
    /// <returns></returns>
    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForAccessoriesAsync()
    {
        return await _context.Accessories
            .Where(x => x.IsActive && !x.IsDeleted)
            .Select(x => new DropDownDto<int>
            {
                Key = x.Id,
                Text = x.Name
            }).ToListAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Binds the drop down for categories asynchronous.
    /// </summary>
    /// <returns></returns>
    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForCategoriesAsync()
    {
        return await _context.Categories
            .Where(x => !x.IsDeleted)
            .Select(x => new DropDownDto<int>
            {
                Key = x.Id,
                Text = x.Name
            }).ToListAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Binds the drop down for orders asynchronous.
    /// </summary>
    /// <returns></returns>
    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForOrdersAsync()
    {
        return await _context.Orders
            .Where(x => !x.IsDeleted)
            .Select(x => new DropDownDto<int>
            {
                Key = x.Id,
                Text = "Order #" + x.Id + " - " + x.Customer.FirstName + " " + x.Customer.LastName
            }).ToListAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Binds the drop down for customers asynchronous.
    /// </summary>
    /// <returns></returns>
    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForCustomersAsync()
    {
        return await _context.Customers
            .Where(x => x.IsActive && !x.IsDeleted)
            .Select(x => new DropDownDto<int>
            {
                Key = x.Id,
                Text = x.FirstName + " " + x.LastName
            }).ToListAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Binds the drop down for products asynchronous.
    /// </summary>
    /// <returns></returns>
    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForProductsAsync()
    {
        return await _context.Products
            .Where(x => x.IsActive && !x.IsDeleted)
            .Select(x => new DropDownDto<int>
            {
                Key = x.Id,
                Text = x.Name + " - " + x.ModelNo
            }).ToListAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Binds the drop down for status.
    /// </summary>
    /// <returns></returns>
    IList<DropDownDto<byte>> IDropDownService.BindDropDownForOrderStatus()
    {
        return OrderStatus;
    }

    /// <summary>
    /// The order status
    /// </summary>
    private static readonly IList<DropDownDto<byte>> OrderStatus =
        ((byte[])Enum.GetValuesAsUnderlyingType<OrderStatus>())
        .Select(x => new DropDownDto<byte>
        {
            Key = x,
            Text = ((OrderStatus)x).ToString()
        }).ToArray();

    /// <summary>
    /// Binds the drop down for order status.
    /// </summary>
    /// <returns></returns>
    IList<DropDownDto<byte>> IDropDownService.BindDropDownForOrderDetailStatus()
    {
        return OrderDetailStatus;
    }

    /// <summary>
    /// The order detail status
    /// </summary>
    private static readonly IList<DropDownDto<byte>> OrderDetailStatus =
        ((byte[])Enum.GetValuesAsUnderlyingType<OrderDetailStatus>())
        .Select(x => new DropDownDto<byte>
        {
            Key = x,
            Text = ((OrderDetailStatus)x).ToString()
        }).ToArray();

    /// <summary>
    /// Binds the drop down for service.
    /// </summary>
    /// <returns></returns>
    IList<DropDownDto<byte>> IDropDownService.BindDropDownForService()
    {
        return Services;
    }

    /// <summary>
    /// The services
    /// </summary>
    private static readonly IList<DropDownDto<byte>> Services =
        ((byte[])Enum.GetValuesAsUnderlyingType<ServiceStatus>())
        .Select(x => new DropDownDto<byte>
        {
            Key = x,
            Text = ((ServiceStatus)x).ToString()
        }).ToArray();
}