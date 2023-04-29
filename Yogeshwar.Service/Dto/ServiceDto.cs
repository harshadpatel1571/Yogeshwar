namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class ServiceDto.
/// Implements the <see cref="BaseDto" />
/// </summary>
/// <seealso cref="BaseDto" />
public sealed class ServiceDto : BaseDto
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the worker.
    /// </summary>
    /// <value>The name of the worker.</value>
    [Required(ErrorMessage = "Worker name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Worker name must be 3 to 50 character long.")]
    public string WorkerName { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    [StringLength(int.MaxValue, MinimumLength = 5, ErrorMessage = "Description must be at least 5 character long.")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the order identifier.
    /// </summary>
    /// <value>The order identifier.</value>
    [Required(ErrorMessage = "Order is required.")]
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the name of the order customer.
    /// </summary>
    /// <value>The name of the order customer.</value>
    public string? OrderCustomerName { get; set; }

    /// <summary>
    /// Gets or sets the service status.
    /// </summary>
    /// <value>The service status.</value>
    [Required(ErrorMessage = "Status is required.")]
    public byte ServiceStatus { get; set; }

    /// <summary>
    /// Gets or sets the service status string.
    /// </summary>
    /// <value>The service status string.</value>
    public string? ServiceStatusString { get; set; }

    /// <summary>
    /// Gets or sets the complain date.
    /// </summary>
    /// <value>The complain date.</value>
    public string? ComplainDate { get; set; }

    /// <summary>
    /// Gets or sets the completed date.
    /// </summary>
    /// <value>The completed date.</value>
    public DateTime? CompletedDate { get; set; }
}
