namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class CategoryDto.
/// Implements the <see cref="BaseDto" />
/// </summary>
/// <seealso cref="BaseDto" />
public sealed class CategoryDto : BaseDto
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
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be 3 to 50 character long.")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the HSN no.
    /// </summary>
    /// <value>The HSN no.</value>
    [Required(ErrorMessage = "HsnNo is required.")]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "HsnNo must be 1 to 10 character long.")]
    public string HsnNo { get; set; }

    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets the image file.
    /// </summary>
    /// <value>The image file.</value>
    [ValidateFile]
    public IFormFile? ImageFile { get; set; }
}