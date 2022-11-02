namespace Yogeshwar.Service.Service;

internal class NotificationService : INotificationService
{
    private readonly YogeshwarContext _context;

    public NotificationService(YogeshwarContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    async Task<DataTableResponseCarrier<NotificationDto>> INotificationService.GetByFilterAsync(DataTableFilterDto filterDto)
    {
        try
        {
            var result = _context.Notifications.AsNoTracking();

            if (!string.IsNullOrEmpty(filterDto.SearchValue))
            {
                result = result.Where(x => x.ProductAccessories.Product.Name.Contains(filterDto.SearchValue) ||
                                           x.ProductAccessories.Accessories.Name.Contains(filterDto.SearchValue));
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

            var data = await result.OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder)
                .Include(x => x.ProductAccessories).ThenInclude(x => x.Product).Include(x=>x.ProductAccessories.Accessories)
                .Select(x => DtoSelector(x)).ToListAsync().ConfigureAwait(false);

            model.Data = data;

            return model;
        }
        catch(Exception e)
        {
            return null;
        }
    }

    private static NotificationDto DtoSelector(Notification service) =>
    new()
    {
        Id = service.Id,
        ProductId = service.ProductAccessories.ProductId,
        ProductName = service.ProductAccessories.Product.Name,
        OrderId = service.OrderId,
        ProductAccessoriesId= service.ProductAccessoriesId,
        ProductAccessoriesName = service.ProductAccessories.Accessories.Name,
        IsCompleted = service.IsCompleted,
        StrIsCompleted = service.IsCompleted ? "Completed" : "Pending"
    };
}
