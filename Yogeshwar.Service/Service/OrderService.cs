//namespace Yogeshwar.Service.Service;

///// <summary>
///// Class OrderService.
///// Implements the <see cref="IOrderService" />
///// </summary>
///// <seealso cref="IOrderService" />
//[RegisterService(ServiceLifetime.Scoped, typeof(IOrderService))]
//internal sealed class OrderService : IOrderService
//{
//    /// <summary>
//    /// The context
//    /// </summary>
//    private readonly YogeshwarContext _context;

//    /// <summary>
//    /// The current user service
//    /// </summary>
//    private readonly ICurrentUserService _currentUserService;

//    /// <summary>
//    /// The product image read path
//    /// </summary>
//    private readonly string _productImageReadPath;

//    /// <summary>
//    /// The accessories image read path
//    /// </summary>
//    private readonly string _accessoriesImageReadPath;

//    /// <summary>
//    /// The customer image read path
//    /// </summary>
//    private readonly string _customerImageReadPath;

//    /// <summary>
//    /// Initializes a new instance of the <see cref="OrderService" /> class.
//    /// </summary>
//    /// <param name="context">The context.</param>
//    /// <param name="configuration">The configuration.</param>
//    /// <param name="currentUserService">The current user service.</param>
//    public OrderService(YogeshwarContext context, IConfiguration configuration,
//        ICurrentUserService currentUserService)
//    {
//        _context = context;
//        _currentUserService = currentUserService;
//        _productImageReadPath = configuration["File:ReadPath"] + "/Product";
//        _accessoriesImageReadPath = configuration["File:ReadPath"] + "/Accessories";
//        _customerImageReadPath = configuration["File:ReadPath"] + "/Customer";
//    }

//    /// <summary>
//    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
//    /// </summary>
//    public void Dispose()
//    {
//        _context.Dispose();
//        GC.SuppressFinalize(this);
//    }

//    /// <summary>
//    /// Gets the by filter asynchronous.
//    /// </summary>
//    /// <param name="filterDto">The filter dto.</param>
//    /// <param name="cancellationToken">The cancellation token.</param>
//    /// <returns>Task&lt;DataTableResponseCarrier&lt;OrderDto&gt;&gt;.</returns>
//    async Task<DataTableResponseCarrier<OrderDto>> IOrderService.GetByFilterAsync(DataTableFilterDto filterDto,
//        CancellationToken cancellationToken)
//    {
//        var result = _context.Orders.AsNoTracking();

//        if (!string.IsNullOrEmpty(filterDto.SearchValue))
//        {
//            result = result.Where(x => x.OrderDetails.Select(c => c.Product.Name)
//                                           .Contains(filterDto.SearchValue) ||
//                                       x.Customer.FirstName.StartsWith(filterDto.SearchValue));
//        }

//        var model = new DataTableResponseCarrier<OrderDto>
//        {
//            TotalCount = result.Count()
//        };

//        result = result.Skip(filterDto.Skip);

//        if (filterDto.Take != -1)
//        {
//            result = result.Take(filterDto.Take);
//        }

//        IList<OrderDto> data = await result
//            .Include(x => x.Customer)
//            .Include(x => x.OrderDetails)
//            .Select(x => DtoSelector(x))
//            .ToListAsync(cancellationToken)
//            .ConfigureAwait(false);

//        data = data.AsQueryable().OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder).ToArray();

//        model.Data = data;

//        return model;
//    }

//    /// <summary>
//    /// Dto selector.
//    /// </summary>
//    /// <param name="order">The order.</param>
//    /// <returns>OrderDto.</returns>
//    private static OrderDto DtoSelector(Order order)
//    {
//        return new OrderDto
//        {
//            Id = order.Id,
//            CustomerName = order.Customer.FirstName + " " + order.Customer.LastName,
//            OrderDate = order.OrderDate.ToString("dd-MM-yyyy"),
//            IsCompleted = order.IsCompleted,
//            Amount = order.OrderDetails.Sum(c => c.Amount) - (order.Discount ?? 0),
//            OrderCount = order.OrderDetails.Count
//        };
//    }

//    /// <summary>
//    /// Create or update as an asynchronous operation.
//    /// </summary>
//    /// <param name="orderDto">The order dto.</param>
//    /// <param name="cancellationToken">The cancellation token.</param>
//    /// <returns>A Task&lt;OneOf`3&gt; representing the asynchronous operation.</returns>
//    //public async Task<OneOf<int, NotFound, string[]>> CreateOrUpdateAsync(OrderDto orderDto,
//    //    CancellationToken cancellationToken)
//    //{
//    //    // if (orderDto.Id < 1)
//    //    // {
//    //    return await CreateAsync(orderDto, cancellationToken).ConfigureAwait(false);
//    //    // }
//    //    //
//    //    // return await UpdateAsync(orderDto).ConfigureAwait(false);
//    //}

//    /// <summary>
//    /// Create as an asynchronous operation.
//    /// </summary>
//    /// <param name="orderDto">The order dto.</param>
//    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
//    /// <returns>A Task&lt;OneOf`3&gt; representing the asynchronous operation.</returns>
//    //private async Task<OneOf<int, NotFound, string[]>> CreateAsync(OrderDto orderDto,
//    //    CancellationToken cancellationToken)
//    //{
        

//    //    return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
//    //}

//    // private async Task<OneOf<int, NotFound, string[]>> UpdateAsync(OrderDto orderDto)
//    // {
//    //     var dbModel = await _context.Orders
//    //         .FirstOrDefaultAsync(c => c.Id == orderDto.Id && !c.IsDeleted)
//    //         .ConfigureAwait(false);
//    //
//    //     if (dbModel is null)
//    //     {
//    //         return new NotFound();
//    //     }
//    //     
//    //     
//    // }

//    /// <summary>
//    /// Get product details as an asynchronous operation.
//    /// </summary>
//    /// <param name="productIds">The product ids.</param>
//    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
//    /// <returns>A Task&lt;System.Object&gt; representing the asynchronous operation.</returns>
//    private async Task<object> GetProductDetailsAsync(IEnumerable<int> productIds, CancellationToken cancellationToken)
//    {
//        return await _context.Products
//            .Where(x => productIds.Contains(x.Id) && x.IsActive && !x.IsDeleted)
//            .Select(x => new ProductDetailDto
//            {
//                Id = x.Id,
//                AccessoriesDetails = x.ProductAccessories
//                    .Select(c => new AccessoriesDetailDto2
//                    {
//                        Id = c.AccessoryId,
//                        Quantity = c.Quantity
//                    }).ToArray(),
//                Name = x.Name + "-" + x.ModelNo,
//                Price = x.Price
//            })
//            .ToDictionaryAsync(x => x.Id, x => x, cancellationToken)
//            .ConfigureAwait(false);
//    }

//    /// <summary>
//    /// Get accessories details as an asynchronous operation.
//    /// </summary>
//    /// <param name="productIds">The product ids.</param>
//    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
//    /// <returns>A Task&lt;System.Object&gt; representing the asynchronous operation.</returns>
//    private async Task<object> GetAccessoriesDetailsAsync(IEnumerable<int> productIds,
//        CancellationToken cancellationToken)
//    {
//        return await _context.Products.Where(c => productIds.Contains(c.Id))
//            .SelectMany(c => c.ProductAccessories)
//            .Select(c => new AccessoriesDetailDto2
//            {
//                Id = c.Id,
//                Quantity = c.Quantity
//            }).ToDictionaryAsync(x => x.Id, x => x, cancellationToken)
//            .ConfigureAwait(false);
//    }

//    /// <summary>
//    /// Set product quantities as an asynchronous operation.
//    /// </summary>
//    /// <param name="productDetails">The product details.</param>
//    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
//    /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
//    private async Task<int> SetProductQuantitiesAsync(object productDetails, CancellationToken cancellationToken)
//    {
//        var productDetailsDic = (Dictionary<int, ProductDetailDto>)productDetails;

//        var products = await _context.Products
//            .Where(x => productDetailsDic.Keys.Contains(x.Id))
//            .ToListAsync(cancellationToken).ConfigureAwait(false);

//        // foreach (var product in products)
//        //     product.Quantity = productDetailsDic[product.Id].Quantity;

//        _context.Products.UpdateRange(products);

//        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
//    }

//    /// <summary>
//    /// Gets the accessories asynchronous.
//    /// </summary>
//    /// <param name="productId">The product identifier.</param>
//    /// <param name="cancellationToken">The cancellation token.</param>
//    /// <returns>Task&lt;System.Nullable&lt;ProductAccessoriesDetailDto&gt;&gt;.</returns>
//    //async Task<ProductAccessoriesDetailDto?> IOrderService.GetAccessoriesAsync(int productId,
//    //    CancellationToken cancellationToken)
//    //{
//    //    return await _context.Products.Where(x => x.Id == productId)
//    //        .Select(x => new ProductAccessoriesDetailDto
//    //        {
//    //            Image = x.ProductImages.Count < 1
//    //                ? null
//    //                : $"{_productImageReadPath}/{x.ProductImages.FirstOrDefault().Image}",
//    //            Amount = x.Price,
//    //            Accessories = x.ProductAccessories.Select(c => new AccessoriesDetailDto
//    //            {
//    //                Id = c.AccessoriesId,
//    //                Name = c.Accessories.Name,
//    //                Image = c.Accessories.Image == null ? null : $"{_accessoriesImageReadPath}/{c.Accessories.Image}"
//    //            }).ToArray()
//    //        }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
//    //}

//    /// <summary>
//    /// Delete as an asynchronous operation.
//    /// </summary>
//    /// <param name="id">The identifier.</param>
//    /// <param name="cancellationToken">The cancellation token.</param>
//    /// <returns>A Task&lt;Order&gt; representing the asynchronous operation.</returns>
//    public async Task<Order?> DeleteAsync(int id, CancellationToken cancellationToken)
//    {
//        var dbModel = await _context.Orders
//            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
//            .ConfigureAwait(false);

//        if (dbModel == null)
//        {
//            return null;
//        }

//        dbModel.IsDeleted = true;

//        _context.Orders.Update(dbModel);

//        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

//        return dbModel;
//    }

//    /// <summary>
//    /// Get details as an asynchronous operation.
//    /// </summary>
//    /// <param name="id">The identifier.</param>
//    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
//    /// <returns>A Task&lt;OrderDetailViewModel&gt; representing the asynchronous operation.</returns>
//    public async Task<OrderDetailViewModel?> GetDetailsAsync(int id, CancellationToken cancellationToken) =>
//        await _context.Orders.Where(x => x.Id == id)
//            .Select(x => new OrderDetailViewModel
//            {
//                OrderId = "Order #" + x.Id,
//                Customer = new CustomerViewDto
//                {
//                    Id = x.CustomerId,
//                    Name = x.Customer.FirstName + " " + x.Customer.LastName,
//                    Email = x.Customer.Email,
//                    PhoneNo = x.Customer.PhoneNo,
//                    //Address = x.Customer.Address + ", " + x.Customer.City + " - " + x.Customer.PinCode + ".",
//                    Image = x.Customer.Image == null ? null : $"{_customerImageReadPath}/{x.Customer.Image}"
//                },
//                OrderDetails = x.OrderDetails.Select(c => new OrderDetailsViewDto
//                {
//                    ProductName = c.Product.Name + " - " + c.Product.ModelNo,
//                    Status = ((OrderDetailStatus)c.Status).ToString(),
//                    Quantity = c.Quantity,
//                    TotalAmount = c.Amount + " RS",
//                    DeliveredDate = c.ReceiveDate == null
//                        ? "Not delivered"
//                        : c.ReceiveDate.Value.ToString("dd-MM-yyyy"),
//                    Price = (c.Amount / c.Quantity).ToString("0.00") + " RS",
//                    Image = c.Product.ProductImages.Count < 1
//                        ? null
//                        : $"{_productImageReadPath}/{c.Product.ProductImages.First().Image}"
//                }).ToArray(),
//                SubTotal = x.OrderDetails.Sum(c => c.Amount) + " RS",
//                Discount = x.Discount == null ? "0 RS" : x.Discount.Value + " RS",
//                Total = x.OrderDetails.Sum(c => c.Amount) - (x.Discount ?? 0) + " RS"
//            }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
//}