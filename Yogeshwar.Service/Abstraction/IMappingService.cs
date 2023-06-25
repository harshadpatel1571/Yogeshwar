namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface IMappingService
/// </summary>
internal interface IMappingService
{
    /// <summary>
    /// Maps the specified accessory.
    /// </summary>
    /// <param name="accessory">The accessory.</param>
    /// <returns>AccessoriesDto.</returns>
    internal AccessoriesDto Map(Accessory accessory);

    /// <summary>
    /// Maps the specified customer.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns>CustomerDto.</returns>
    internal CustomerDto Map(Customer customer);

    /// <summary>
    /// Maps the specified customer address.
    /// </summary>
    /// <param name="customerAddress">The customer address.</param>
    /// <returns>CustomerAddressDto.</returns>
    internal CustomerAddressDto Map(CustomerAddress customerAddress);

    /// <summary>
    /// Maps the specified category.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns>CategoryDto.</returns>
    internal CategoryDto Map(Category category);

    /// <summary>
    /// Maps the specified product.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <returns>ProductDto.</returns>
    internal ProductDto Map(Product product);

    /// <summary>
    /// Maps the specified product accessory.
    /// </summary>
    /// <param name="productAccessory">The product accessory.</param>
    /// <returns>ProductAccessoryDto.</returns>
    internal ProductAccessoryDto Map(ProductAccessory productAccessory);

    /// <summary>
    /// Maps the specified product category.
    /// </summary>
    /// <param name="productCategory">The product category.</param>
    /// <returns>ProductCategoryDto.</returns>
    internal ProductCategoryDto Map(ProductCategory productCategory);

    /// <summary>
    /// Maps the specified product image.
    /// </summary>
    /// <param name="productImage">The product image.</param>
    /// <returns>ProductImageDto.</returns>
    internal ProductImageDto Map(ProductImage productImage);

    /// <summary>
    /// Maps the specified configuration.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>ConfigurationDto.</returns>
    internal ConfigurationDto Map(Configuration configuration);

    /// <summary>
    /// Maps the specified order.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <returns>OrderDto.</returns>
    internal OrderDto Map(Order order);

    /// <summary>
    /// Maps the specified order detail.
    /// </summary>
    /// <param name="orderDetail">The order detail.</param>
    /// <returns>OrderDetailDto.</returns>
    internal OrderDetailDto Map(OrderDetail orderDetail);
}