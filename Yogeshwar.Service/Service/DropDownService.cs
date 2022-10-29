namespace Yogeshwar.Service.Service;

internal class DropDownService : IDropDownService
{
    private readonly YogeshwarContext _context;

    public DropDownService(YogeshwarContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForAccessoriesAsync()
    {
        return await _context.Accessories.Select(x => new DropDownDto<int>
        {
            Key = x.Id,
            Text = x.Name
        }).ToListAsync().ConfigureAwait(false);
    }

    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForCustomersAsync()
    {
        return await _context.Customers.Select(x => new DropDownDto<int>
        {
            Key = x.Id,
            Text = x.FirstName + " " + x.LastName
        }).ToListAsync().ConfigureAwait(false);
    }

    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForProductsAsync()
    {
        return await _context.Products.Select(x => new DropDownDto<int>
        {
            Key = x.Id,
            Text = x.Name + " - " + x.ModelNo
        }).ToListAsync().ConfigureAwait(false);
    }

    IList<DropDownDto<byte>> IDropDownService.BindDropDownForStatus()
    {
        return ((byte[])Enum.GetValuesAsUnderlyingType<OrderStatus>()).Select(x => new DropDownDto<byte>
        {
            Key = x,
            Text = ((OrderStatus)x).ToString()
        }).ToArray();
    }

    IList<DropDownDto<byte>> IDropDownService.BindDropDownForOrderStatus()
    {
        return ((byte[])Enum.GetValuesAsUnderlyingType<OrderDetailStatus>()).Select(x => new DropDownDto<byte>
        {
            Key = x,
            Text = ((OrderDetailStatus)x).ToString()
        }).ToArray();
    }

    IList<DropDownDto<byte>> IDropDownService.BindDropDownForService()
    {
        return ((byte[])Enum.GetValuesAsUnderlyingType<ServiceStatus>()).Select(x => new DropDownDto<byte>
        {
            Key = x,
            Text = ((ServiceStatus)x).ToString()
        }).ToArray();
    }
}