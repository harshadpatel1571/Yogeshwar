namespace Yogeshwar.DB.DbModels;

/// <summary>
/// Class Notification. This class cannot be inherited.
/// </summary>
[Table(nameof(Notification))]
internal sealed class Notification
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the product accessories identifier.
    /// </summary>
    /// <value>The product accessories identifier.</value>
    public int ProductAccessoriesId { get; set; }

    /// <summary>
    /// Gets or sets the order identifier.
    /// </summary>
    /// <value>The order identifier.</value>
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is completed.
    /// </summary>
    /// <value><c>true</c> if this instance is completed; otherwise, <c>false</c>.</value>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    /// <value>The date.</value>
    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the order.
    /// </summary>
    /// <value>The order.</value>
    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; }

    /// <summary>
    /// Gets or sets the product accessories.
    /// </summary>
    /// <value>The product accessories.</value>
    [ForeignKey(nameof(ProductAccessoriesId))]
    public ProductAccessory ProductAccessories { get; set; }
}