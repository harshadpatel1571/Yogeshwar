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
    
    internal ProductDto Map(Product product);
    
    internal Product Map(ProductDto productDto);
}