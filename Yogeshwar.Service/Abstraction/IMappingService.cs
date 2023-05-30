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
    /// Maps the specified accessory dto.
    /// </summary>
    /// <param name="accessoryDto">The accessory dto.</param>
    /// <returns>Accessory.</returns>
    internal Accessory Map(AccessoriesDto accessoryDto);

    /// <summary>
    /// Maps the specified customer.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns>CustomerDto.</returns>
    internal CustomerDto Map(Customer customer);

    /// <summary>
    /// Maps the specified customer dto.
    /// </summary>
    /// <param name="customerDto">The customer dto.</param>
    /// <returns>Customer.</returns>
    internal Customer Map(CustomerDto customerDto);

    /// <summary>
    /// Maps the specified customer address.
    /// </summary>
    /// <param name="customerAddress">The customer address.</param>
    /// <returns>CustomerAddressDto.</returns>
    internal CustomerAddressDto Map(CustomerAddress customerAddress);

    /// <summary>
    /// Maps the specified customer address dto.
    /// </summary>
    /// <param name="customerAddressDto">The customer address dto.</param>
    /// <returns>CustomerAddress.</returns>
    internal CustomerAddress Map(CustomerAddressDto customerAddressDto);

    /// <summary>
    /// Maps the specified category.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns>CategoryDto.</returns>
    internal CategoryDto Map(Category category);

    /// <summary>
    /// Maps the specified category dto.
    /// </summary>
    /// <param name="categoryDto">The category dto.</param>
    /// <returns>Category.</returns>
    internal Category Map(CategoryDto categoryDto);

    /// <summary>
    /// Maps the specified product.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <returns>ProductDto.</returns>
    internal ProductDto Map(Product product);

    /// <summary>
    /// Maps the specified product dto.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <returns>Product.</returns>
    internal Product Map(ProductDto productDto);

    /// <summary>
    /// Maps the specified product accessory.
    /// </summary>
    /// <param name="productAccessory">The product accessory.</param>
    /// <returns>ProductAccessoryDto.</returns>
    internal ProductAccessoryDto Map(ProductAccessory productAccessory);

    /// <summary>
    /// Maps the specified product accessory dto.
    /// </summary>
    /// <param name="productAccessoryDto">The product accessory dto.</param>
    /// <returns>ProductAccessory.</returns>
    internal ProductAccessory Map(ProductAccessoryDto productAccessoryDto);

    /// <summary>
    /// Maps the specified product category dto.
    /// </summary>
    /// <param name="productCategoryDto">The product category dto.</param>
    /// <returns>ProductCategory.</returns>
    internal ProductCategory Map(ProductCategoryDto productCategoryDto);

    /// <summary>
    /// Maps the specified product category.
    /// </summary>
    /// <param name="productCategory">The product category.</param>
    /// <returns>ProductCategoryDto.</returns>
    internal ProductCategoryDto Map(ProductCategory productCategory);

    /// <summary>
    /// Maps the specified product image dto.
    /// </summary>
    /// <param name="productImageDto">The product image dto.</param>
    /// <returns>ProductImage.</returns>
    internal ProductImage Map(ProductImageDto productImageDto);

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
}