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

    async Task<IList<DropDownDto<int>>> IDropDownService.BindDropDownForAccessories()
    {
        return await _context.Accessories.Select(x => new DropDownDto<int>
        {
            Key = x.Id,
            Text = x.Name
        }).ToListAsync().ConfigureAwait(false);
    }
}