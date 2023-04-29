namespace Yogeshwar.Service.Dto;

public sealed class ProductAccessoryDto
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the product identifier.
    /// </summary>
    /// <value>The product identifier.</value>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the accessories identifier.
    /// </summary>
    /// <value>The accessories identifier.</value>
    [Required(ErrorMessage = "Accessory is required.")]
    public int AccessoryId { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    [Required(ErrorMessage = "Quantity is required.")]
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the accessories.
    /// </summary>
    /// <value>The accessories.</value>
    public AccessoriesDto? Accessory { get; set; }

    /// <summary>
    /// Gets or sets the product.
    /// </summary>
    /// <value>The product.</value>
    public ProductDto? Product { get; set; }
}