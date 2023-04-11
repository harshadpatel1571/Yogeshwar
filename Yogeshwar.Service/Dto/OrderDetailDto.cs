using System.Diagnostics;

namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class OrderDetailDto.
/// Implements the <see cref="BaseDto" />
/// </summary>
/// <seealso cref="BaseDto" />
public class OrderDetailDto : BaseDto
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
    [Required(ErrorMessage = "Product is required.")]
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    [Required(ErrorMessage = "Quantity is required.")]
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the amount.
    /// </summary>
    /// <value>The amount.</value>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the delivered date.
    /// </summary>
    /// <value>The delivered date.</value>
    public string? deliveredDate { get; set; }

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    /// <value>The status.</value>
    [Required(ErrorMessage = "Status is required.")]
    public OrderDetailStatus? Status { get; set; }
}

/// <summary>
/// Class CustomerViewDto.
/// </summary>
public class CustomerViewDto
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public required int Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the phone no.
    /// </summary>
    /// <value>The phone no.</value>
    public required string PhoneNo { get; set; }

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    /// <value>The address.</value>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    public string? Image { get; set; }
}

/// <summary>
/// Class OrderDetailsViewDto.
/// </summary>
[DebuggerDisplay("Price is {Price}")]
public class OrderDetailsViewDto
{
    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    /// <value>The name of the product.</value>
    public required string ProductName { get; set; }

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    /// <value>The status.</value>
    public required string Status { get; set; }

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>The price.</value>
    public required string Price { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    public required int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the total amount.
    /// </summary>
    /// <value>The total amount.</value>
    public required string TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the delivered date.
    /// </summary>
    /// <value>The delivered date.</value>
    public required string DeliveredDate { get; set; }
}

/// <summary>
/// Class OrderDetailViewModel.
/// </summary>
public class OrderDetailViewModel
{
    /// <summary>
    /// Gets or sets the order identifier.
    /// </summary>
    /// <value>The order identifier.</value>
    public required string OrderId { get; set; }

    /// <summary>
    /// Gets or sets the customer.
    /// </summary>
    /// <value>The customer.</value>
    public required CustomerViewDto Customer { get; set; }

    /// <summary>
    /// Gets or sets the order details.
    /// </summary>
    /// <value>The order details.</value>
    public required IList<OrderDetailsViewDto> OrderDetails { get; set; }

    /// <summary>
    /// Gets or sets the sub total.
    /// </summary>
    /// <value>The sub total.</value>
    public required string SubTotal { get; set; }

    /// <summary>
    /// Gets or sets the discount.
    /// </summary>
    /// <value>The discount.</value>
    public required string Discount { get; set; }

    /// <summary>
    /// Gets or sets the total.
    /// </summary>
    /// <value>The total.</value>
    public required string Total { get; set; }
}