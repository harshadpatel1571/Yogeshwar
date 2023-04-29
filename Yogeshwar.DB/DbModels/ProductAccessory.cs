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
    /// Gets or sets the accessory identifier.
    /// </summary>
    /// <value>The accessory identifier.</value>
    public int AccessoryId { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the accessory.
    /// </summary>
    /// <value>The accessory.</value>
    [ForeignKey(nameof(AccessoryId))]
    public Accessory Accessory { get; set; }

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