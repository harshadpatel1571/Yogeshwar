namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class NotificationDto.
/// Implements the <see cref="BaseDto" />
/// </summary>
/// <seealso cref="BaseDto" />
public class NotificationDto : BaseDto
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
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    /// <value>The name of the product.</value>
    public string ProductName { get; set; }

    /// <summary>
    /// Gets or sets the order identifier.
    /// </summary>
    /// <value>The order identifier.</value>
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the product accessories identifier.
    /// </summary>
    /// <value>The product accessories identifier.</value>
    public int ProductAccessoriesId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product accessories.
    /// </summary>
    /// <value>The name of the product accessories.</value>
    public string ProductAccessoriesName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is completed.
    /// </summary>
    /// <value><c>true</c> if this instance is completed; otherwise, <c>false</c>.</value>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Gets or sets the string is completed.
    /// </summary>
    /// <value>The string is completed.</value>
    public string StrIsCompleted { get; set; }
}
