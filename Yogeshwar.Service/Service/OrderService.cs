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

        IList<OrderDto> data = await result.Include(x => x.Customer).Include(x => x.OrderDetails)
            .Select(x => DtoSelector(x)).ToListAsync().ConfigureAwait(false);

        data = data.AsQueryable().OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder).ToArray();

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
            Amount = order.OrderDetails.Sum(c => c.Amount) - (order.Discount ?? 0),
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
        var orderDetails = orderDto.OrderDetails.Select(x => new OrderDetail
        {
            ProductId = x.ProductId,
            Amount = _context.Products.Where(y => y.Id == x.ProductId).Select(c => c.Price).FirstOrDefault() * x.Quantity,
            Status = (byte)x.Status!,
            Quantity = x.Quantity,
            ReceiveDate = x.deliveredDate == null
                ? null
                : DateTime.ParseExact(x.deliveredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
        }).ToArray();

        var notifications = orderDto.OrderDetails.Select(x => new { x.ProductId, x.Accessories })
            .SelectMany(x => x.Accessories!.Where(c => !c.IsSelected)
                .Select(c => new Notification
                {
                    ProductAccessoriesId = _context.ProductAccessories
                        .Where(y => y.ProductId == x.ProductId && y.AccessoriesId == c.Id)
                        .Select(o => o.Id)
                        .FirstOrDefault(),
                    Date = DateTime.Now
                })).ToArray();

        var dbModel = new Order
        {
            CustomerId = orderDto.CustomerId!.Value,
            Discount = orderDto.Discount,
            OrderDate = DateTime.ParseExact(orderDto.OrderDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
            OrderDetails = orderDetails,
            Notifications = notifications
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

        dbModel.CustomerId = orderDto.CustomerId!.Value;
        dbModel.Discount = orderDto.Discount;
        dbModel.OrderDate = DateTime.ParseExact(orderDto.OrderDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

        foreach (var dbModelOrderDetail in dbModel.OrderDetails)
        {
            var orderDetail = orderDto.OrderDetails.FirstOrDefault(x => x.Id == dbModelOrderDetail.Id);

            if (orderDetail is null)
            {
                continue;
            }

            dbModelOrderDetail.Amount = orderDetail.Amount;
            dbModelOrderDetail.Status = (byte)orderDetail.Status!;
            dbModelOrderDetail.Quantity = orderDetail.Quantity;
            if (orderDetail.deliveredDate != null)
            {
                dbModelOrderDetail.ReceiveDate = DateTime.ParseExact(orderDetail.deliveredDate, "dd-MM-yyyy",
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
                    : $"{_productImageReadPath}/{x.ProductImages.FirstOrDefault().Image}",
                Amount = x.Price,
                Accessories = x.ProductAccessories.Select(c => new AccessoriesDetailDto
                {
                    Id = c.AccessoriesId,
                    Name = c.Accessories.Name,
                    Image = c.Accessories.Image == null ? null : $"{_accessoriesImageReadPath}/{c.Accessories.Image}"
                }).ToArray()
            }).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<OrderDetailViewModel?> GetDetailsAsync(int id) =>
        await _context.Orders.Where(x => x.Id == id)
            .Select(x => new OrderDetailViewModel
            {
                OrderId = "Order #" + x.Id,
                Customer = new CustomerViewDto
                {
                    Id = x.CustomerId,
                    Name = x.Customer.FirstName + " " + x.Customer.LastName,
                    Email = x.Customer.Email,
                    PhoneNo = x.Customer.PhoneNo,
                    Address = x.Customer.Address + ", " + x.Customer.City + " - " + x.Customer.Pincode + "."
                },
                OrderDetails = x.OrderDetails.Select(c => new OrderDetailsViewDto
                {
                    ProductName = c.Product.Name + " - " + c.Product.ModelNo,
                    Status = ((OrderDetailStatus)c.Status).ToString(),
                    Quantity = c.Quantity,
                    TotalAmount = c.Amount + " RS",
                    DeliveredDate = c.ReceiveDate == null
                        ? "Not delivered"
                        : c.ReceiveDate.Value.ToString("dd-MM-yyyy"),
                    Price = (c.Amount / c.Quantity).ToString("0.00") + " RS",
                    Image = c.Product.ProductImages.Count < 1
                        ? null
                        : $"{_productImageReadPath}/{c.Product.ProductImages.First().Image}"
                }).ToArray(),
                SubTotal = x.OrderDetails.Sum(c => c.Amount) + " RS",
                Discount = x.Discount == null ? "0 RS" : x.Discount.Value + " RS",
                Total = x.OrderDetails.Sum(c => c.Amount) - (x.Discount ?? 0) + " RS"
            }).FirstOrDefaultAsync().ConfigureAwait(false);
}