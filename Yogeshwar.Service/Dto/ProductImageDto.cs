namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class ProductImageDto.
/// </summary>
public sealed class ProductImageDto
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
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    public string Image { get; set; }
}