namespace Yogeshwar.DB.DbModels;

/// <summary>
/// Class ProductAccessory. This class cannot be inherited.
/// </summary>
[Table("ProductAccessories")]
internal sealed class ProductAccessory
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [Key]
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
    public int AccessoriesId { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the accessories.
    /// </summary>
    /// <value>The accessories.</value>
    [ForeignKey(nameof(AccessoriesId))]
    public Accessory Accessories { get; set; }

    /// <summary>
    /// Gets or sets the product.
    /// </summary>
    /// <value>The product.</value>
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }

    /// <summary>
    /// Gets the notifications.
    /// </summary>
    /// <value>The notifications.</value>
    public IList<Notification> Notifications { get; } = new List<Notification>();
}