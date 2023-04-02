namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class ProductDto.
/// Implements the <see cref="BaseDto" />
/// </summary>
/// <seealso cref="BaseDto" />
public class ProductDto : BaseDto
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be 3 to 100 character long.")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(int.MaxValue, MinimumLength = 5, ErrorMessage = "Description must be at least 5 character long.")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>The price.</value>
    [Required(ErrorMessage = "Price is required.")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Gets or sets the model no.
    /// </summary>
    /// <value>The model no.</value>
    [Required(ErrorMessage = "Model no is required.")]
    [StringLength(50, ErrorMessage = "Model no must be up to 50 character long.")]
    public string ModelNo { get; set; }

    /// <summary>
    /// Gets or sets the accessories.
    /// </summary>
    /// <value>The accessories.</value>
    [Required(ErrorMessage = "Accessories are required.")]
    public IList<int> Accessories { get; set; }

    /// <summary>
    /// Gets or sets the categories.
    /// </summary>
    /// <value>The categories.</value>
    [Required(ErrorMessage = "Categories are required.")]
    public IList<int> Categories { get; set; }

    /// <summary>
    /// Gets or sets the accessories quantity.
    /// </summary>
    /// <value>The accessories quantity.</value>
    [Required(ErrorMessage = "Accessories quantities are required.")]
    public IList<AccessoriesQuantity> AccessoriesQuantity { get; set; }

    /// <summary>
    /// Gets the images.
    /// </summary>
    /// <value>The images.</value>
    internal IList<ImageIds>? Images { get; set; }

    /// <summary>
    /// Gets or sets the select lists for accessories.
    /// </summary>
    /// <value>The select lists for accessories.</value>
    public SelectList? SelectListsForAccessories { get; set; }

    /// <summary>
    /// Gets or sets the select lists for categories.
    /// </summary>
    /// <value>The select lists for categories.</value>
    public SelectList? SelectListsForCategories { get; set; }

    /// <summary>
    /// Gets or sets the video.
    /// </summary>
    /// <value>The video.</value>
    public string? Video { get; set; }

    /// <summary>
    /// Gets or sets the image files.
    /// </summary>
    /// <value>The image files.</value>
    [ValidateFile]
    public IList<IFormFile>? ImageFiles { get; set; }

    /// <summary>
    /// Gets or sets the video file.
    /// </summary>
    /// <value>The video file.</value>
    [ValidateFile]
    public IFormFile? VideoFile { get; set; }
}

/// <summary>
/// Class AccessoriesQuantity.
/// </summary>
public class AccessoriesQuantity
{
    /// <summary>
    /// Gets or sets the accessories identifier.
    /// </summary>
    /// <value>The accessories identifier.</value>
    public int AccessoriesId { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets the image.
    /// </summary>
    /// <value>The image.</value>
    internal string? Image { get; set; }
}

/// <summary>
/// Class ImageIds.
/// </summary>
internal class ImageIds
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    public string Image { get; set; }
}

/// <summary>
/// Class ProductAccessoriesDto.
/// </summary>
public class ProductAccessoriesDto
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
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    public int Quantity { get; set; }
}

/// <summary>
/// Class ProductAccessoriesDetailDto.
/// </summary>
internal class ProductAccessoriesDetailDto
{
    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets the accessories.
    /// </summary>
    /// <value>The accessories.</value>
    public IList<AccessoriesDetailDto> Accessories { get; set; }

    /// <summary>
    /// Gets or sets the amount.
    /// </summary>
    /// <value>The amount.</value>
    public decimal Amount { get; set; }
}

public record struct AccessoriesDetailDto
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    public string? Image { get; set; }
}