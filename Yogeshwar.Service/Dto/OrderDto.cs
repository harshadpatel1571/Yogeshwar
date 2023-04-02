namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class OrderDto.
/// Implements the <see cref="BaseDto" />
/// </summary>
/// <seealso cref="BaseDto" />
public class OrderDto : BaseDto
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    /// <value>The customer identifier.</value>
    [Required(ErrorMessage = "Customer is required.")]
    public int? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the discount.
    /// </summary>
    /// <value>The discount.</value>
    public decimal? Discount { get; set; }

    /// <summary>
    /// Gets or sets the amount.
    /// </summary>
    /// <value>The amount.</value>
    public decimal? Amount { get; set; }

    /// <summary>
    /// Gets or sets the name of the customer.
    /// </summary>
    /// <value>The name of the customer.</value>
    public string? CustomerName { get; set; }

    //[Required(ErrorMessage = "Order status is required.")]
    //public OrderStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the order date.
    /// </summary>
    /// <value>The order date.</value>
    [Required(ErrorMessage = "Order date is required.")]
    public string OrderDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is completed.
    /// </summary>
    /// <value><c>true</c> if this instance is completed; otherwise, <c>false</c>.</value>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [force create].
    /// </summary>
    /// <value><c>true</c> if [force create]; otherwise, <c>false</c>.</value>
    public bool ForceCreate { get; set; }

    /// <summary>
    /// Gets or sets the order count.
    /// </summary>
    /// <value>The order count.</value>
    public int OrderCount { get; set; }

    /// <summary>
    /// Gets or sets the order details.
    /// </summary>
    /// <value>The order details.</value>
    [Required(ErrorMessage = "Order details are required.")]
    public IList<OrderDetailDto> OrderDetails { get; set; }
}