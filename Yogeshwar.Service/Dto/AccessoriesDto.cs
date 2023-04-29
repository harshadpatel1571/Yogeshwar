namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class AccessoriesDto.
/// Implements the <see cref="BaseDto" />
/// </summary>
/// <seealso cref="BaseDto" />
public sealed class AccessoriesDto : BaseDto
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
    /// Gets or sets the type of the measurement.
    /// </summary>
    /// <value>The type of the measurement.</value>
    [Required(ErrorMessage = "Measurement type is required.")]
    [StringLength(10, MinimumLength = 2, ErrorMessage = "Measurement type must be 2 to 10 character long.")]
    public string MeasurementType { get; set; }

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>The price.</value>
    [Required(ErrorMessage = "Price is required.")]
    [Range(1, 9999999999.999999, ErrorMessage = "Price must be greater than 1.")]
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    [StringLength(int.MaxValue, MinimumLength = 5, ErrorMessage = "Description must be at least 5 character long.")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    [Required(ErrorMessage = "Quantity is required.")]
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the file.
    /// </summary>
    /// <value>The file.</value>
    [ValidateFile]
    public IFormFile? ImageFile { get; set; }
}