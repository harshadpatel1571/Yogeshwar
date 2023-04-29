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
    /// Maps the specified accessory dto.
    /// </summary>
    /// <param name="accessoryDto">The accessory dto.</param>
    /// <returns>Accessory.</returns>
    Accessory IMappingService.Map(AccessoriesDto accessoryDto) => InternalMapper.Map(accessoryDto);

    /// <summary>
    /// Maps the specified customer.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <returns>CustomerDto.</returns>
    CustomerDto IMappingService.Map(Customer customer) => InternalMapper.Map(customer);

    /// <summary>
    /// Maps the specified customer dto.
    /// </summary>
    /// <param name="customerDto">The customer dto.</param>
    /// <returns>Customer.</returns>
    Customer IMappingService.Map(CustomerDto customerDto) => InternalMapper.Map(customerDto);

    /// <summary>
    /// Maps the specified customer address.
    /// </summary>
    /// <param name="customerAddress">The customer address.</param>
    /// <returns>CustomerAddressDto.</returns>
    CustomerAddressDto IMappingService.Map(CustomerAddress customerAddress) => InternalMapper.Map(customerAddress);

    /// <summary>
    /// Maps the specified customer address dto.
    /// </summary>
    /// <param name="customerAddressDto">The customer address dto.</param>
    /// <returns>CustomerAddress.</returns>
    CustomerAddress IMappingService.Map(CustomerAddressDto customerAddressDto) => InternalMapper.Map(customerAddressDto);

    /// <summary>
    /// Maps the specified category.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns>CategoryDto.</returns>
    CategoryDto IMappingService.Map(Category category) => InternalMapper.Map(category);

    /// <summary>
    /// Maps the specified category dto.
    /// </summary>
    /// <param name="categoryDto">The category dto.</param>
    /// <returns>Category.</returns>
    Category IMappingService.Map(CategoryDto categoryDto) => InternalMapper.Map(categoryDto);

    /// <summary>
    /// Maps the specified product.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <returns>ProductDto.</returns>
    ProductDto IMappingService.Map(Product product) => InternalMapper.Map(product);

    /// <summary>
    /// Maps the specified product dto.
    /// </summary>
    /// <param name="productDto">The product dto.</param>
    /// <returns>Product.</returns>
    Product IMappingService.Map(ProductDto productDto) => InternalMapper.Map(productDto);

    /// <summary>
    /// Maps the specified product accessory.
    /// </summary>
    /// <param name="productAccessory">The product accessory.</param>
    /// <returns>ProductAccessoryDto.</returns>
    ProductAccessoryDto IMappingService.Map(ProductAccessory productAccessory) => InternalMapper.Map(productAccessory);

    /// <summary>
    /// Maps the specified product accessory dto.
    /// </summary>
    /// <param name="productAccessoryDto">The product accessory dto.</param>
    /// <returns>ProductAccessory.</returns>
    ProductAccessory IMappingService.Map(ProductAccessoryDto productAccessoryDto) => InternalMapper.Map(productAccessoryDto);

    /// <summary>
    /// Maps the specified product category dto.
    /// </summary>
    /// <param name="productCategoryDto">The product category dto.</param>
    /// <returns>ProductCategory.</returns>
    ProductCategory IMappingService.Map(ProductCategoryDto productCategoryDto) => InternalMapper.Map(productCategoryDto);

    /// <summary>
    /// Maps the specified product category.
    /// </summary>
    /// <param name="productCategory">The product category.</param>
    /// <returns>ProductCategoryDto.</returns>
    ProductCategoryDto IMappingService.Map(ProductCategory productCategory) => InternalMapper.Map(productCategory);

    /// <summary>
    /// Maps the specified product image dto.
    /// </summary>
    /// <param name="productImageDto">The product image dto.</param>
    /// <returns>ProductImage.</returns>
    ProductImage IMappingService.Map(ProductImageDto productImageDto) => InternalMapper.Map(productImageDto);

    /// <summary>
    /// Maps the specified product image.
    /// </summary>
    /// <param name="productImage">The product image.</param>
    /// <returns>ProductImageDto.</returns>
    ProductImageDto IMappingService.Map(ProductImage productImage) => InternalMapper.Map(productImage);
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
}