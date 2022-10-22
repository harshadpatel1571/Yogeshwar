namespace Yogeshwar.Service.Abstraction;

public interface IOrderService : IDisposable
{
    internal Task<DataTableResponseCarrier<OrderDto>> GetByFilterAsync(DataTableFilterDto filterDto);

    internal Task<ProductAccessoriesDetailDto?> GetAccessoriesAsync(int productId);
    
    Task<int> CreateOrUpdateAsync(OrderDto orderDto);

    Task<Order?> DeleteAsync(int id);
}