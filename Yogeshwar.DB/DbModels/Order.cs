namespace Yogeshwar.DB.DbModels;

/// <summary>
/// Class Order. This class cannot be inherited.
/// </summary>
[Table(nameof(Order))]
internal sealed class Order
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    /// <value>The customer identifier.</value>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the discount.
    /// </summary>
    /// <value>The discount.</value>
    public decimal? Discount { get; set; }

    /// <summary>
    /// Gets or sets the order date.
    /// </summary>
    /// <value>The order date.</value>
    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Gets or sets the amount.
    /// </summary>
    /// <value>The amount.</value>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is completed.
    /// </summary>
    /// <value><c>true</c> if this instance is completed; otherwise, <c>false</c>.</value>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
    public bool IsActive { get; set; }

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
    /// Gets or sets the customer.
    /// </summary>
    /// <value>The customer.</value>
    public Customer Customer { get; set; }

    /// <summary>
    /// Gets or sets the customer services.
    /// </summary>
    /// <value>The customer services.</value>
    public IList<CustomerService> CustomerServices { get; set; } = new List<CustomerService>();

    /// <summary>
    /// Gets or sets the notifications.
    /// </summary>
    /// <value>The notifications.</value>
    public IList<Notification> Notifications { get; set; } = new List<Notification>();

    /// <summary>
    /// Gets or sets the order details.
    /// </summary>
    /// <value>The order details.</value>
    public IList<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}