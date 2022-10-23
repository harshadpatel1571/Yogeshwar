using System.Globalization;

namespace Yogeshwar.Service.Service;

internal class OrderService : IOrderService
{
    private readonly YogeshwarContext _context;
    private static string _productImageReadPath;
    private static string _accessoriesImageReadPath;

    public OrderService(YogeshwarContext context, IConfiguration configuration)
    {
        _context = context;
        _productImageReadPath = configuration["File:ReadPath"] + "/Product";
        _accessoriesImageReadPath = configuration["File:ReadPath"] + "/Accessories";
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    async Task<DataTableResponseCarrier<OrderDto>> IOrderService.GetByFilterAsync(DataTableFilterDto filterDto)
    {
        var result = _context.Orders.AsNoTracking();

        if (!string.IsNullOrEmpty(filterDto.SearchValue))
        {
            result = result.Where(x => x.OrderDetails.Select(c => c.Product.Name).Contains(filterDto.SearchValue) ||
                                       x.Customer.FirstName.StartsWith(filterDto.SearchValue));
        }

        var model = new DataTableResponseCarrier<OrderDto>
        {
            TotalCount = result.Count(),
        };

        result = result.Skip(filterDto.Skip);

        if (filterDto.Take != -1)
        {
            result = result.Take(filterDto.Take);
        }

        var data = await result.OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder)
            .Select(x => DtoSelector(x)).ToListAsync().ConfigureAwait(false);

        model.Data = data;

        return model;
    }

    private static OrderDto DtoSelector(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            CustomerName = order.Customer.FirstName + " " + order.Customer.LastName,
            OrderDate = order.OrderDate.ToString("dd-MM-yyyy"),
            IsCompleted = order.IsCompleted,
            Amount = order.OrderDetails.Sum(c => c.Amount),
            OrderCount = order.OrderDetails.Count
        };
    }

    public async Task<int> CreateOrUpdateAsync(OrderDto orderDto)
    {
        if (orderDto.Id < 1)
        {
            return await CreateAsync(orderDto).ConfigureAwait(false);
        }

        return await UpdateAsync(orderDto).ConfigureAwait(false);
    }

    private async ValueTask<int> CreateAsync(OrderDto orderDto)
    {
        var dbModel = new Order
        {
            CustomerId = orderDto.CustomerId,
            Discount = orderDto.Discount,
            OrderDate = DateTime.ParseExact(orderDto.OrderDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
            OrderDetails = orderDto.OrderDetail.Select(x => new OrderDetail
            {
                ProductId = x.ProductId,
                Amount = x.Amount,
                Status = (byte)x.Status,
                Quantity = x.Quantity
            }).ToArray()
        };

        await _context.Orders.AddAsync(dbModel).ConfigureAwait(false);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    private async ValueTask<int> UpdateAsync(OrderDto orderDto)
    {
        var dbModel = await _context.Orders
            .Include(x => x.OrderDetails)
            .FirstOrDefaultAsync(x => x.Id == orderDto.Id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return 0;
        }

        dbModel.CustomerId = orderDto.CustomerId;
        dbModel.Discount = orderDto.Discount;
        dbModel.OrderDate = DateTime.ParseExact(orderDto.OrderDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        foreach (var dbModelOrderDetail in dbModel.OrderDetails)
        {
            var orderDetail = orderDto.OrderDetail.FirstOrDefault(x => x.Id == dbModelOrderDetail.Id);

            if (orderDetail is null)
            {
                continue;
            }

            dbModelOrderDetail.Amount = orderDetail.Amount;
            dbModelOrderDetail.Status = (byte)orderDetail.Status;
            dbModelOrderDetail.Quantity = orderDetail.Quantity;
            if (orderDetail.ReceivedDate != null)
            {
                dbModelOrderDetail.ReceiveDate = DateTime.ParseExact(orderDetail.ReceivedDate, "dd-MM-yyyy",
                    CultureInfo.InvariantCulture);
            }
        }

        _context.OrderDetails.UpdateRange(dbModel.OrderDetails);
        _context.Orders.Update(dbModel);

        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<Order?> DeleteAsync(int id)
    {
        var dbModel = await _context.Orders
            .Include(x => x.OrderDetails)
            .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

        if (dbModel == null)
        {
            return null;
        }

        _context.OrderDetails.RemoveRange(dbModel.OrderDetails);
        _context.Orders.Remove(dbModel);

        await _context.SaveChangesAsync().ConfigureAwait(false);

        return dbModel;
    }

    async Task<ProductAccessoriesDetailDto?> IOrderService.GetAccessoriesAsync(int productId)
    {
        return await _context.Products.Where(x => x.Id == productId)
            .Select(x => new ProductAccessoriesDetailDto
            {
                Image = x.ProductImages.Count < 1
                    ? null
                    : $"/DataImages/Product/{x.ProductImages.FirstOrDefault().Image}",
                Amount = x.Price,
                Accessories = x.ProductAccessories.Select(c => new AccessoriesDetailDto
                {
                    Id = c.AccessoriesId,
                    Name = c.Accessories.Name,
                    Image = c.Accessories.Image == null ? null : $"/DataImages/Accessories/{c.Accessories.Image}"
                }).ToArray()
            }).FirstOrDefaultAsync();
    }
}