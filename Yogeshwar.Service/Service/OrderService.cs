namespace Yogeshwar.Service.Service;

/// <summary>
/// Class OrderService.
/// Implements the <see cref="IOrderService" />
/// </summary>
/// <seealso cref="IOrderService" />
[RegisterService(ServiceLifetime.Scoped, typeof(IOrderService))]
internal sealed class OrderService : IOrderService
{
    /// <summary>
    /// The context
    /// </summary>
    private readonly YogeshwarContext _context;

    /// <summary>
    /// The current user service
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// The mapping service
    /// </summary>
    private readonly IMappingService _mappingService;

    /// <summary>
    /// The caching service
    /// </summary>
    private readonly ICachingService _cachingService;

    public OrderService(YogeshwarContext context, ICurrentUserService currentUserService,
        IMappingService mappingService, ICachingService cachingService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _mappingService = mappingService;
        _cachingService = cachingService;
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
    /// Gets the by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;DataTableResponseCarrier&lt;OrderDto&gt;&gt;.</returns>
    public async Task<DataTableResponseCarrier<OrderDto>> GetByFilterAsync(DataTableFilterDto filterDto,
        CancellationToken cancellationToken)
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
            TotalCount = result.Count()
        };

        result = result.Skip(filterDto.Skip);

        if (filterDto.Take != -1)
        {
            result = result.Take(filterDto.Take);
        }

        var data = await result
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .Include(x => x.Customer)
            .ThenInclude(x => x.CustomerAddresses)
            .Select(x => _mappingService.Map(x))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        model.Data = data.AsQueryable().OrderBy(filterDto.SortColumn + " " + filterDto.SortOrder).ToArray();

        return model;
    }

    /// <summary>
    /// Get by identifier as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;OrderDto&gt; representing the asynchronous operation.</returns>
    public async Task<OrderDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.Where(x => x.Id == id && !x.IsDeleted)
            .AsNoTracking()
            .Include(x => x.OrderDetails)
            .Include(x => x.Customer)
            .ThenInclude(x => x.CustomerAddresses)
            .Select(x => _mappingService.Map(x))
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (order is null)
        {
            return null;
        }

        var products = await _cachingService.GetProductsAsync(cancellationToken).ConfigureAwait(false);

        order.OrderDetails.ForEach(od =>
        {
            od.Product = products.FirstOrDefault(p => p.Id == od.ProductId)!;
        });

        return order;
    }

    /// <summary>
    /// Create or update as an asynchronous operation.
    /// </summary>
    /// <param name="orderDto">The order dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;OneOf`2&gt; representing the asynchronous operation.</returns>
    public async Task<OneOf<int, NotFound>> CreateOrUpdateAsync(OrderDto orderDto, CancellationToken cancellationToken)
    {
        if (orderDto.Id < 1)
        {
            return await CreateAsync(orderDto, cancellationToken).ConfigureAwait(false);
        }

        return await UpdateAsync(orderDto, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Create as an asynchronous operation.
    /// </summary>
    /// <param name="orderDto">The order dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;OneOf`2&gt; representing the asynchronous operation.</returns>
    private async Task<OneOf<int, NotFound>> CreateAsync(OrderDto orderDto, CancellationToken cancellationToken)
    {
        var products = await _cachingService.GetProductsAsync(cancellationToken).ConfigureAwait(false);

        var orderDetails = new List<OrderDetail>();

        foreach (var orderDetail in orderDto.OrderDetails)
        {
            var product = products.FirstOrDefault(x => x.Id == orderDetail.ProductId)
                          ?? throw new InvalidDataArgumentException($"Product for given id '{orderDetail.ProductId}' does not exist anymore.");

            var newOrderDetail = new OrderDetail
            {
                CreatedDate = DateTime.Now,
                CreatedBy = _currentUserService.GetCurrentUserId(),
                Amount = product.Price * orderDetail.Quantity,
                ProductId = product.Id,
                Status = (byte)orderDetail.Status,
                Quantity = orderDetail.Quantity,
                ReceiveDate = orderDetail.ReceiveDate
            };

            orderDetails.Add(newOrderDetail);
        }

        var order = new Order
        {
            CreatedDate = DateTime.Now,
            CreatedBy = _currentUserService.GetCurrentUserId(),
            OrderDetails = orderDetails,
            Amount = orderDetails.Sum(x => x.Amount),
            CustomerId = orderDto.CustomerId,
            Discount = orderDto.Discount,
            OrderDate = orderDto.OrderDate
        };

        await _context.Orders.AddAsync(order, cancellationToken).ConfigureAwait(false);

        try
        {
            return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Update as an asynchronous operation.
    /// </summary>
    /// <param name="orderDto">The order dto.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;OneOf`2&gt; representing the asynchronous operation.</returns>
    /// <exception cref="Yogeshwar.Helper.Domain.InvalidDataArgumentException">Product for given id '{item.ProductId}' does not exist anymore.</exception>
    private async Task<OneOf<int, NotFound>> UpdateAsync(OrderDto orderDto, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Orders
            .Include(x => x.OrderDetails)
            .FirstOrDefaultAsync(c => c.Id == orderDto.Id && !c.IsDeleted, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel is null)
        {
            return new NotFound();
        }

        var existingOrderDetails = dbModel.OrderDetails;

        var products = await _cachingService.GetProductsAsync(cancellationToken).ConfigureAwait(false);

        var updateOrderDetails = new List<OrderDetail>();
        var insertOrderDetails = new List<OrderDetail>();

        foreach (var orderDetail in orderDto.OrderDetails)
        {
            var product = products.FirstOrDefault(x => x.Id == orderDetail.ProductId)
                ?? throw new InvalidDataArgumentException($"Product for given id '{orderDetail.ProductId}' does not exist anymore.");

            var existingOrderDetail = existingOrderDetails.FirstOrDefault(x => x.Id == orderDetail.Id);

            if (existingOrderDetail is not null)
            {
                existingOrderDetail.ProductId = product.Id;
                existingOrderDetail.Amount = product.Price * orderDetail.Quantity;
                existingOrderDetail.Status = (byte)orderDetail.Status;
                existingOrderDetail.ReceiveDate = orderDetail.ReceiveDate;
                existingOrderDetail.ModifiedDate = DateTime.Now;
                existingOrderDetail.ModifiedBy = _currentUserService.GetCurrentUserId();

                updateOrderDetails.Add(existingOrderDetail);
                existingOrderDetails.Remove(existingOrderDetail);
            }
            else
            {
                var newOrderDetail = new OrderDetail
                {
                    OrderId = dbModel.Id,
                    CreatedDate = DateTime.Now,
                    CreatedBy = _currentUserService.GetCurrentUserId(),
                    Amount = product.Price * orderDetail.Quantity,
                    ProductId = product.Id,
                    Status = (byte)orderDetail.Status,
                    Quantity = orderDetail.Quantity,
                    ReceiveDate = orderDetail.ReceiveDate
                };

                insertOrderDetails.Add(newOrderDetail);
            }
        }

        dbModel.Discount = orderDto.Discount;
        dbModel.OrderDate = orderDto.OrderDate;
        dbModel.Amount = insertOrderDetails.Sum(x => x.Amount) + updateOrderDetails.Sum(x => x.Amount);

        await _context.OrderDetails.AddRangeAsync(insertOrderDetails, cancellationToken).ConfigureAwait(false);
        _context.OrderDetails.UpdateRange(updateOrderDetails);
        _context.OrderDetails.RemoveRange(existingOrderDetails);

        _context.Orders.Update(dbModel);

        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Delete as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task&lt;Order&gt; representing the asynchronous operation.</returns>
    public async Task<OrderDto?> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var dbModel = await _context.Orders
            .Include(x => x.OrderDetails)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (dbModel == null)
        {
            return null;
        }

        dbModel.IsDeleted = true;
        dbModel.ModifiedDate = DateTime.Now;

        dbModel.OrderDetails.ForEach(x =>
        {
            x.IsDeleted = true;
            x.ModifiedDate = DateTime.Now;
        });

        _context.Orders.Update(dbModel);
        _context.OrderDetails.UpdateRange(dbModel.OrderDetails);

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return _mappingService.Map(dbModel);
    }
}