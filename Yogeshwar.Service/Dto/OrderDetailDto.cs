using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
    /// Gets or sets the order identifier.
    /// </summary>
    /// <value>The order identifier.</value>
    public int OrderId { get; set; }

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
    /// Gets or sets the receive date.
    /// </summary>
    /// <value>The receive date.</value>
    public DateTime? ReceiveDate { get; set; }

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    /// <value>The status.</value>
    [Required(ErrorMessage = "Status is required.")]
    public OrderDetailStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the product.
    /// </summary>
    /// <value>The product.</value>
    [ValidateNever]
    public ProductDto Product { get; set; }
}