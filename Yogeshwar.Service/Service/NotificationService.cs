namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(INotificationService))]
internal class NotificationService : INotificationService
{
    private readonly YogeshwarContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public NotificationService(YogeshwarContext context)
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
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <returns></returns>
    async Task<DataTableResponseCarrier<NotificationDto>> INotificationService.GetByFilterAsync(
        DataTableFilterDto filterDto)
    {
        var result = _context.Notifications.AsNoTracking();

        if (!string.IsNullOrEmpty(filterDto.SearchValue))
        {
            result = result.Where(x => x.ProductAccessories.Product.Name.Contains(filterDto.SearchValue) ||
                                       x.ProductAccessories.Accessories.Name.Contains(filterDto.SearchValue) ||
                                       x.OrderId.ToString() == filterDto.SearchValue);
        }

        var model = new DataTableResponseCarrier<NotificationDto>
        {
            TotalCount = result.Count()
        };

        result = result.Skip(filterDto.Skip);

        if (filterDto.Take != -1)
        {
            result = result.Take(filterDto.Take);
        }

        IList<NotificationDto> data = await result
            .Include(x => x.ProductAccessories)
            .ThenInclude(x => x.Product)
            .Include(x => x.ProductAccessories.Accessories)
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
    private static NotificationDto DtoSelector(Notification service) => new()
    {
        Id = service.Id,
        ProductId = service.ProductAccessories.ProductId,
        ProductName = service.ProductAccessories.Product.Name,
        OrderId = service.OrderId,
        ProductAccessoriesId = service.ProductAccessoriesId,
        ProductAccessoriesName = service.ProductAccessories.Accessories.Name,
        IsCompleted = service.IsCompleted,
        StrIsCompleted = service.IsCompleted ? "Completed" : "Pending"
    };
}