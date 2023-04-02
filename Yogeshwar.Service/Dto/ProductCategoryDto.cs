namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class ProductCategoryDto.
/// Implements the <see cref="BaseDto" />
/// </summary>
/// <seealso cref="BaseDto" />
public class ProductCategoryDto : BaseDto
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the category identifier.
    /// </summary>
    /// <value>The category identifier.</value>
    [Required(ErrorMessage = "Category is required.")]
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the product identifier.
    /// </summary>
    /// <value>The product identifier.</value>
    [Required(ErrorMessage = "Product is required.")]
    public int ProductId { get; set; }
}