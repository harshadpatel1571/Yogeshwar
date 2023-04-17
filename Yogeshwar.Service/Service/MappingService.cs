namespace Yogeshwar.Service.Service;

/// <summary>
/// Class MappingService.
/// Implements the <see cref="Yogeshwar.Service.Abstraction.IMappingService" />
/// </summary>
/// <seealso cref="Yogeshwar.Service.Abstraction.IMappingService" />
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
    
    ProductDto IMappingService.Map(Product product) => InternalMapper.Map(product);
    
    Product IMappingService.Map(ProductDto productDto) => InternalMapper.Map(productDto);
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
    
    internal static partial ProductDto Map(Product product);

    internal static partial Product Map(ProductDto productDto);
}