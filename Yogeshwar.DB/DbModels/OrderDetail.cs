namespace Yogeshwar.DB.DbModels;

/// <summary>
/// Class OrderDetail. This class cannot be inherited.
/// </summary>
[Table("OrderDetails")]
internal sealed class OrderDetail
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the order identifier.
    /// </summary>
    /// <value>The order identifier.</value>
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the product identifier.
    /// </summary>
    /// <value>The product identifier.</value>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the amount.
    /// </summary>
    /// <value>The amount.</value>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the receive date.
    /// </summary>
    /// <value>The receive date.</value>
    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime? ReceiveDate { get; set; }

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    /// <value>The status.</value>
    public byte Status { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is deleted.
    /// </summary>
    /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the created date.
    /// </summary>
    /// <value>The created date.</value>
    [Column(TypeName = GlobalDataType.DateDataType)]

    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the created by.
    /// </summary>
    /// <value>The created by.</value>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the modified date.
    /// </summary>
    /// <value>The modified date.</value>
    [Column(TypeName = GlobalDataType.DateDataType)]

    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets the modified by.
    /// </summary>
    /// <value>The modified by.</value>
    public int? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the order.
    /// </summary>
    /// <value>The order.</value>
    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; }

    /// <summary>
    /// Gets or sets the product.
    /// </summary>
    /// <value>The product.</value>
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }
}