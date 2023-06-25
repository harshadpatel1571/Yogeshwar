namespace Yogeshwar.Service.Service;

/// <summary>
/// Class MappingService.
/// Implements the <see cref="IMappingService" />
/// </summary>
/// <seealso cref="IMappingService" />
[RegisterService(ServiceLifetime.Singleton, typeof(IMappingService))]
internal class MappingService : IMappingService
{
    /// <summary>
    /// Maps the specified accessory.
    /// </summary>
    /// <param name="accessory">The accessory.</param>
    /// <returns>AccessoriesDto.</returns>
    AccessoriesDto IMappingService.Map(Accessory accessory) => InternalMapper.Map(accessory);

    /// <summary>
    /// Maps the specified customer.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns>CustomerDto.</returns>
    CustomerDto IMappingService.Map(Customer customer) => InternalMapper.Map(customer);

    /// <summary>
    /// Maps the specified customer address.
    /// </summary>
    /// <param name="customerAddress">The customer address.</param>
    /// <returns>CustomerAddressDto.</returns>
    CustomerAddressDto IMappingService.Map(CustomerAddress customerAddress) => InternalMapper.Map(customerAddress);

    /// <summary>
    /// Maps the specified category.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns>CategoryDto.</returns>
    CategoryDto IMappingService.Map(Category category) => InternalMapper.Map(category);

    /// <summary>
    /// Maps the specified product.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <returns>ProductDto.</returns>
    ProductDto IMappingService.Map(Product product) => InternalMapper.Map(product);

    /// <summary>
    /// Maps the specified product accessory.
    /// </summary>
    /// <param name="productAccessory">The product accessory.</param>
    /// <returns>ProductAccessoryDto.</returns>
    ProductAccessoryDto IMappingService.Map(ProductAccessory productAccessory) => InternalMapper.Map(productAccessory);

    /// <summary>
    /// Maps the specified product category.
    /// </summary>
    /// <param name="productCategory">The product category.</param>
    /// <returns>ProductCategoryDto.</returns>
    ProductCategoryDto IMappingService.Map(ProductCategory productCategory) => InternalMapper.Map(productCategory);

    /// <summary>
    /// Maps the specified product image.
    /// </summary>
    /// <param name="productImage">The product image.</param>
    /// <returns>ProductImageDto.</returns>
    ProductImageDto IMappingService.Map(ProductImage productImage) => InternalMapper.Map(productImage);

    /// <summary>
    /// Maps the specified configuration.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>ConfigurationDto.</returns>
    ConfigurationDto IMappingService.Map(Configuration configuration) => InternalMapper.Map(configuration);

    /// <summary>
    /// Maps the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>OrderDto.</returns>
    OrderDto IMappingService.Map(Order order) => InternalMapper.Map(order);

    /// <summary>
    /// Maps the specified order detail.
    /// </summary>
    /// <param name="orderDetail">The order detail.</param>
    /// <returns>OrderDetailDto.</returns>
    OrderDetailDto IMappingService.Map(OrderDetail orderDetail) => InternalMapper.Map(orderDetail);
}

/// <summary>
/// Class InternalMapper.
/// </summary>
[Mapper]
internal static partial class InternalMapper
{
    /// <summary>
    /// Maps the specified accessory.
    /// </summary>
    /// <param name="accessory">The accessory.</param>
    /// <returns>AccessoriesDto.</returns>
    internal static partial AccessoriesDto Map(Accessory accessory);

    /// <summary>
    /// Maps the specified accessory.
    /// </summary>
    /// <param name="accessory">The accessory.</param>
    /// <returns>Accessory.</returns>
    internal static partial Accessory Map(AccessoriesDto accessory);

    /// <summary>
    /// Maps the specified customer.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns>CustomerDto.</returns>
    internal static partial CustomerDto Map(Customer customer);

    /// <summary>
    /// Maps the specified customer dto.
    /// </summary>
    /// <param name="customerDto">The customer dto.</param>
    /// <returns>Customer.</returns>
    internal static partial Customer Map(CustomerDto customerDto);

    /// <summary>
    /// Maps the specified customer address.
    /// </summary>
    /// <param name="customerAddress">The customer address.</param>
    /// <returns>CustomerAddressDto.</returns>
    internal static partial CustomerAddressDto Map(CustomerAddress customerAddress);

    /// <summary>
    /// Maps the specified customer address dto.
    /// </summary>
    /// <param name="customerAddressDto">The customer address dto.</param>
    /// <returns>CustomerAddress.</returns>
    internal static partial CustomerAddress Map(CustomerAddressDto customerAddressDto);

    /// <summary>
    /// Maps the specified category.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns>CategoryDto.</returns>
    internal static partial CategoryDto Map(Category category);

    /// <summary>
    /// Maps the specified category dto.
    /// </summary>
    /// <param name="categoryDto">The category dto.</param>
    /// <returns>Category.</returns>
    internal static partial Category Map(CategoryDto categoryDto);

    /// <summary>
    /// Maps the specified product.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <returns>ProductDto.</returns>
    internal static partial ProductDto Map(Product product);

    /// <summary>
    /// Maps the specified product dto.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <returns>Product.</returns>
    internal static partial Product Map(ProductDto productDto);

    /// <summary>
    /// Maps the specified product accessory.
    /// </summary>
    /// <param name="productAccessory">The product accessory.</param>
    /// <returns>ProductAccessoryDto.</returns>
    internal static partial ProductAccessoryDto Map(ProductAccessory productAccessory);

    /// <summary>
    /// Maps the specified product accessory dto.
    /// </summary>
    /// <param name="productAccessoryDto">The product accessory dto.</param>
    /// <returns>ProductAccessory.</returns>
    internal static partial ProductAccessory Map(ProductAccessoryDto productAccessoryDto);

    /// <summary>
    /// Maps the specified product category.
    /// </summary>
    /// <param name="productCategory">The product category.</param>
    /// <returns>ProductCategoryDto.</returns>
    internal static partial ProductCategoryDto Map(ProductCategory productCategory);

    /// <summary>
    /// Maps the specified product category dto.
    /// </summary>
    /// <param name="productCategoryDto">The product category dto.</param>
    /// <returns>ProductCategory.</returns>
    internal static partial ProductCategory Map(ProductCategoryDto productCategoryDto);

    /// <summary>
    /// Maps the specified product image.
    /// </summary>
    /// <param name="productImage">The product image.</param>
    /// <returns>ProductImageDto.</returns>
    internal static partial ProductImageDto Map(ProductImage productImage);

    /// <summary>
    /// Maps the specified product image dto.
    /// </summary>
    /// <param name="productImageDto">The product image dto.</param>
    /// <returns>ProductImage.</returns>
    internal static partial ProductImage Map(ProductImageDto productImageDto);

    /// <summary>
    /// Maps the specified configuration.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>ConfigurationDto.</returns>
    internal static partial ConfigurationDto Map(Configuration configuration);

    /// <summary>
    /// Maps the specified configuration dto.
    /// </summary>
    /// <param name="configurationDto">The configuration dto.</param>
    /// <returns>Configuration.</returns>
    internal static partial Configuration Map(ConfigurationDto configurationDto);

    /// <summary>
    /// Maps the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>OrderDto.</returns>
    internal static partial OrderDto Map(Order order);

    /// <summary>
    /// Maps the specified order dto.
    /// </summary>
    /// <param name="orderDto">The order dto.</param>
    /// <returns>Order.</returns>
    internal static partial Order Map(OrderDto orderDto);

    /// <summary>
    /// Maps the specified order detail.
    /// </summary>
    /// <param name="orderDetail">The order detail.</param>
    /// <returns>OrderDetailDto.</returns>
    internal static partial OrderDetailDto Map(OrderDetail orderDetail);

    /// <summary>
    /// Maps the specified order detail dto.
    /// </summary>
    /// <param name="orderDetailDto">The order detail dto.</param>
    /// <returns>OrderDetail.</returns>
    internal static partial OrderDetail Map(OrderDetailDto orderDetailDto);
}