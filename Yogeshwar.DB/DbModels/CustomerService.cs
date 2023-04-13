namespace Yogeshwar.DB.DbModels;

/// <summary>
/// Class CustomerService. This class cannot be inherited.
/// </summary>
[Table(nameof(CustomerService))]
internal sealed class CustomerService
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [Key] public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the worker.
    /// </summary>
    /// <value>The name of the worker.</value>
    [MaxLength(50)] [Unicode(false)] public string WorkerName { get; set; }

    /// <summary>
    /// Gets or sets the order identifier.
    /// </summary>
    /// <value>The order identifier.</value>
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the complain date.
    /// </summary>
    /// <value>The complain date.</value>
    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime ComplainDate { get; set; }

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    /// <value>The status.</value>
    public byte Status { get; set; }

    /// <summary>
    /// Gets or sets the service completion date.
    /// </summary>
    /// <value>The service completion date.</value>
    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime? ServiceCompletionDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is deleted.
    /// </summary>
    /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the order.
    /// </summary>
    /// <value>The order.</value>
    [ForeignKey(nameof(OrderId))] public Order Order { get; set; }
}