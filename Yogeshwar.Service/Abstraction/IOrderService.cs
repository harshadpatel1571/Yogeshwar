namespace Yogeshwar.Service.Abstraction;

public interface IOrderService : IDisposable
{
    internal Task<DataTableResponseCarrier<OrderDto>> GetByFilterAsync(DataTableFilterDto filterDto);

    Task<OneOf<int, NotFound, string[]>> CreateOrUpdateAsync(OrderDto orderDto);

    Task<OrderDetailViewModel?> GetDetailsAsync(int id);

    internal Task<ProductAccessoriesDetailDto?> GetAccessoriesAsync(int productId);

    Task<Order?> DeleteAsync(int id);
}