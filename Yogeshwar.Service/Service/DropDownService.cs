namespace Yogeshwar.Service.Service;

/// <summary>
/// Class DropDownService.
/// Implements the <see cref="IDropDownService" />
/// </summary>
/// <seealso cref="IDropDownService" />
[RegisterService(ServiceLifetime.Scoped, typeof(IDropDownService))]
internal sealed class DropDownService : IDropDownService
{
    /// <summary>
    /// The context
    /// </summary>
    private readonly YogeshwarContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="DropDownService" /> class.
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
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IList&lt;DropDownDto&lt;System.Int32&gt;&gt;&gt;.</returns>
    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForAccessoriesAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Accessories
            .Where(x => x.IsActive && !x.IsDeleted)
            .Select(x => new DropDownDto<int>
            {
                Key = x.Id,
                Text = x.Name
            }).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Binds the drop down for categories asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;IList&lt;DropDownDto&lt;System.Int32&gt;&gt;&gt;.</returns>
    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForCategoriesAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Where(x => !x.IsDeleted && x.IsActive)
            .Select(x => new DropDownDto<int>
            {
                Key = x.Id,
                Text = x.Name
            }).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Binds the drop down for orders asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;IList&lt;DropDownDto&lt;System.Int32&gt;&gt;&gt;.</returns>
    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForOrdersAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Where(x => !x.IsDeleted)
            .Select(x => new DropDownDto<int>
            {
                Key = x.Id,
                Text = "Order #" + x.Id + " - " + x.Customer.FirstName + " " + x.Customer.LastName
            }).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Binds the drop down for customers asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="ids">The ids.</param>
    /// <returns>Task&lt;IList&lt;DropDownDto&lt;System.Int32&gt;&gt;&gt;.</returns>
    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForCustomersAsync(
        CancellationToken cancellationToken, params int[] ids)
    {
        return await _context.Customers
            .Where(x => x.IsActive && !x.IsDeleted && ids.Contains(x.Id))
            .Select(x => new DropDownDto<int>
            {
                Key = x.Id,
                Text = x.FirstName + " " + x.LastName
            }).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Binds the drop down for products asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;IList&lt;DropDownDto&lt;System.Int32&gt;&gt;&gt;.</returns>
    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForProductsAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .Where(x => x.IsActive && !x.IsDeleted)
            .Select(x => new DropDownDto<int>
            {
                Key = x.Id,
                Text = x.Name + " - " + x.ModelNo
            }).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Binds the drop down for status.
    /// </summary>
    /// <returns>IList&lt;DropDownDto&lt;System.Byte&gt;&gt;.</returns>
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
    /// <returns>IList&lt;DropDownDto&lt;System.Byte&gt;&gt;.</returns>
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
    /// <returns>IList&lt;DropDownDto&lt;System.Byte&gt;&gt;.</returns>
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