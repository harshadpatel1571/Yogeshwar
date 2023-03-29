using OneOf;
using OneOf.Types;
using Yogeshwar.DB.Models;

namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(IOrderService))]
internal class OrderService : IOrderService
{
    private readonly YogeshwarContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly string _productImageReadPath;
    private readonly string _accessoriesImageReadPath;
    private readonly string _customerImageReadPath;

    public OrderService(YogeshwarContext context, IConfiguration configuration,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _productImageReadPath = configuration["File:ReadPath"] + "/Product";
        _accessoriesImageReadPath = configuration["File:ReadPath"] + "/Accessories";
        _customerImageReadPath = configuration["File:ReadPath"] + "/Customer";
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
            result = result.Where(x => x.OrderDetails.Select(c => c.Product.Name)
                                           .Contains(filterDto.SearchValue) ||
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

        IList<OrderDto> data = await result.Include(x => x.Customer)
            .Include(x => x.OrderDetails)
            .Select(x => DtoSelector(x))
            .ToListAsync().ConfigureAwait(false);

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

    public async Task<OneOf<int, NotFound, string[]>> CreateOrUpdateAsync(OrderDto orderDto)
    {
        // if (orderDto.Id < 1)
        // {
        return await CreateAsync(orderDto).ConfigureAwait(false);
        // }
        //
        // return await UpdateAsync(orderDto).ConfigureAwait(false);
    }

    private async Task<OneOf<int, NotFound, string[]>> CreateAsync(OrderDto orderDto)
    {
        var productIds = orderDto.OrderDetails.Select(c => c.ProductId);

        var productDetails =
            (Dictionary<int, ProductDetailDto>)await GetProductDetailsAsync(productIds);

        if (!orderDto.ForceCreate)
        {
            foreach (var productDetailDto in productDetails)
            {
                foreach (var valueAccessoriesDetail in productDetailDto.Value.AccessoriesDetails)
                {
                }
            }

            // var errorMessages = orderDto.OrderDetails
            //     .Where(x => productDetails[x.ProductId].AccessoriesDetails.(x => x.Quantity) < x.Quantity)
            //     .Select(c => $"Available quantities for product '{productDetails[c.Id].Name}' is less than asked.")
            //     .ToArray();
            //
            // if (errorMessages.Length > 0)
            //     return errorMessages;
        }

        foreach (var orderDetail in orderDto.OrderDetails)
        {
            var model = productDetails[orderDetail.ProductId];
            //model.Quantity = Math.Max(model.Quantity - orderDetail.Quantity, 0);
        }

        //await SetProductQuantitiesAsync(productDetails);

        var orderDetails = orderDto.OrderDetails
            .Select(x => new OrderDetail
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Status = (byte)x.Status!,
                Amount = productDetails[x.ProductId].Price * x.Quantity
            });

        var order = new Order
        {
            Discount = orderDto.Discount,
            CustomerId = orderDto.CustomerId!.Value,
            OrderDetails = orderDetails.ToArray(),
            CreatedBy = _currentUserService.GetCurrentUserId(),
            CreatedDate = DateTime.Now,
            OrderDate = DateTime.ParseExact(orderDto.OrderDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        };

        _context.Orders.Add(order);

        return await _context.SaveChangesAsync();
    }

    // private async Task<OneOf<int, NotFound, string[]>> UpdateAsync(OrderDto orderDto)
    // {
    //     var dbModel = await _context.Orders
    //         .FirstOrDefaultAsync(c => c.Id == orderDto.Id && !c.IsDeleted)
    //         .ConfigureAwait(false);
    //
    //     if (dbModel is null)
    //     {
    //         return new NotFound();
    //     }
    //     
    //     
    // }

    private async Task<object> GetProductDetailsAsync(IEnumerable<int> productIds)
    {
        return await _context.Products
            .Where(x => productIds.Contains(x.Id) && x.IsActive && !x.IsDeleted)
            .Select(x => new ProductDetailDto
            {
                Id = x.Id,
                AccessoriesDetails = x.ProductAccessories
                    .Select(c => new AccessoriesDetailDto2
                    {
                        Id = c.AccessoriesId,
                        Quantity = c.Quantity
                    }).ToArray(),
                Name = x.Name + "-" + x.ModelNo,
                Price = x.Price
            })
            .ToDictionaryAsync(x => x.Id, x => x)
            .ConfigureAwait(false);
    }

    private async Task<object> GetAccessoriesDetailsAsync(IEnumerable<int> productIds)
    {
        return await _context.Products.Where(c => productIds.Contains(c.Id))
            .SelectMany(c => c.ProductAccessories)
            .Select(c => new AccessoriesDetailDto2
            {
                Id = c.Id,
                Quantity = c.Quantity
            }).ToDictionaryAsync(x => x.Id, x => x)
            .ConfigureAwait(false);
    }

    private async Task<int> SetProductQuantitiesAsync(object productDetails)
    {
        var productDetailsDic = (Dictionary<int, ProductDetailDto>)productDetails;

        var products = await _context.Products
            .Where(x => productDetailsDic.Keys.Contains(x.Id))
            .ToListAsync();

        // foreach (var product in products)
        //     product.Quantity = productDetailsDic[product.Id].Quantity;

        _context.Products.UpdateRange(products);

        return await _context.SaveChangesAsync();
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

    public async Task<Order?> DeleteAsync(int id)
    {
        var dbModel = await _context.Orders
            .FirstOrDefaultAsync(x => x.Id == id)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return null;
        }

        dbModel.IsDeleted = true;

        _context.Orders.Update(dbModel);

        await _context.SaveChangesAsync().ConfigureAwait(false);

        return dbModel;
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
                    Address = x.Customer.Address + ", " + x.Customer.City + " - " + x.Customer.PinCode + ".",
                    Image = x.Customer.Image == null ? null : $"{_customerImageReadPath}/{x.Customer.Image}",
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

file struct ProductDetailDto
{
    public int Id { get; set; }

    public IList<AccessoriesDetailDto2> AccessoriesDetails { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }
}

file struct AccessoriesDetailDto2
{
    public int Id { get; set; }

    public int Quantity { get; set; }
}