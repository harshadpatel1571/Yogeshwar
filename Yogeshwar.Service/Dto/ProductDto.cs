namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class ProductDto.
/// Implements the <see cref="BaseDto" />
/// </summary>
/// <seealso cref="BaseDto" />
public sealed class ProductDto : BaseDto
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
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the model no.
    /// </summary>
    /// <value>The model no.</value>
    [Required(ErrorMessage = "Model no is required.")]
    [StringLength(50, ErrorMessage = "Model no must be up to 50 character long.")]
    public string ModelNo { get; set; }

    /// <summary>
    /// Gets or sets the HSN no.
    /// </summary>
    /// <value>The HSN no.</value>
    [Required(ErrorMessage = "HsnNo is required.")]
    [StringLength(10, ErrorMessage = "HsnNo must be up to 10 character long.")]
    public string HsnNo { get; set; }

    /// <summary>
    /// Gets or sets the GST.
    /// </summary>
    /// <value>The GST.</value>
    [Required(ErrorMessage = "Gst is required.")]
    public decimal Gst { get; set; }

    /// <summary>
    /// Gets or sets the video.
    /// </summary>
    /// <value>The video.</value>
    public string? Video { get; set; }

    /// <summary>
    /// Gets or sets the product accessories.
    /// </summary>
    /// <value>The product accessories.</value>
    public IList<ProductAccessoryDto> ProductAccessories { get; set; }

    /// <summary>
    /// Gets or sets the product categories.
    /// </summary>
    /// <value>The product categories.</value>
    public IList<ProductCategoryDto>? ProductCategories { get; set; }

    /// <summary>
    /// Gets or sets the product images.
    /// </summary>
    /// <value>The product images.</value>
    public IList<ProductImageDto>? ProductImages { get; set; }

    /// <summary>
    /// Gets or sets the accessory ids.
    /// </summary>
    /// <value>The accessory ids.</value>
    [Required(ErrorMessage = "Accessories are required.")]
    public IList<int> AccessoryIds { get; set; }

    /// <summary>
    /// Gets or sets the select lists for accessories.
    /// </summary>
    /// <value>The select lists for accessories.</value>
    public SelectList? SelectListsForAccessories { get; set; }

    /// <summary>
    /// Gets or sets the category ids.
    /// </summary>
    /// <value>The category ids.</value>
    [Required(ErrorMessage = "Categories are required.")]
    public IList<int> CategoryIds { get; set; }

    /// <summary>
    /// Gets or sets the select lists for categories.
    /// </summary>
    /// <value>The select lists for categories.</value>
    public SelectList? SelectListsForCategories { get; set; }

    /// <summary>
    /// Gets or sets the image files.
    /// </summary>
    /// <value>The image files.</value>
    [ValidateFile]
    [JsonIgnore]
    public IList<IFormFile>? ImageFiles { get; set; }

    /// <summary>
    /// Gets or sets the video file.
    /// </summary>
    /// <value>The video file.</value>
    [ValidateFile]
    [JsonIgnore]
    public IFormFile? VideoFile { get; set; }
}