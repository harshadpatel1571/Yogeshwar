namespace Yogeshwar.Helper.Attribute;

/// <summary>
/// Class ValidateFileAttribute. This class cannot be inherited.
/// Implements the <see cref="ValidationAttribute" />
/// </summary>
/// <seealso cref="ValidationAttribute" />
internal sealed class ValidateFileAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets or sets a value indicating whether this instance is required.
    /// </summary>
    /// <value><c>true</c> if this instance is required; otherwise, <c>false</c>.</value>
    internal bool IsRequired { get; set; }

    /// <summary>
    /// Validates the specified value with respect to the current validation attribute.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="validationContext">The context information about the validation operation.</param>
    /// <returns>An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        return value switch
        {
            null => IsRequired
                ? new ValidationResult($"{validationContext.DisplayName} is required.")
                : ValidationResult.Success,
            IFormFile file => ValidateFiles(validationContext, file),
            IEnumerable<IFormFile> files => ValidateFiles(validationContext, files.ToArray()),
            _ => ValidationResult.Success
        };
    }

    /// <summary>
    /// Validates the files.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <param name="files">The files.</param>
    /// <returns>System.Nullable&lt;ValidationResult&gt;.</returns>
    private static ValidationResult? ValidateFiles(ValidationContext validationContext, params IFormFile[] files)
    {
        var fileValidationProperties = validationContext
            .GetService<IConfiguration>()
            .GetSection("Files")
            .Get<FileValidationModel>();

        foreach (var file in files)
        {
            var fileExtension = Path.GetExtension(file.FileName);

            var isImage = fileValidationProperties.ImageExtensions
                .Any(x => string.Equals(fileExtension, x, StringComparison.InvariantCultureIgnoreCase));

            if (!isImage)
            {
                var isVideo = fileValidationProperties.VideoExtensions
                    .Contains(fileExtension);

                if (!isVideo)
                {
                    return new ValidationResult(
                        $"Only '{string.Join(", ",
                            fileValidationProperties.ImageExtensions.Concat(fileValidationProperties.VideoExtensions)
                        ).Replace(".", null)}' files are allowed.");
                }
            }

            var fileSize = file.Length / 1024;
            var maxSize = isImage ? fileValidationProperties.ImageSize : fileValidationProperties.VideoSize;

            if (fileSize > maxSize)
                return new ValidationResult($"File up to size of {maxSize} KB is valid.");
        }

        return ValidationResult.Success;
    }
}

/// <summary>
/// Class FileValidationModel.
/// </summary>
file class FileValidationModel
{
    /// <summary>
    /// Gets or sets the size of the image.
    /// </summary>
    /// <value>The size of the image.</value>
    public int ImageSize { get; set; }

    /// <summary>
    /// Gets or sets the size of the video.
    /// </summary>
    /// <value>The size of the video.</value>
    public int VideoSize { get; set; }

    /// <summary>
    /// Gets or sets the image extensions.
    /// </summary>
    /// <value>The image extensions.</value>
    public string[] ImageExtensions { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Gets or sets the video extensions.
    /// </summary>
    /// <value>The video extensions.</value>
    public string[] VideoExtensions { get; set; } = Array.Empty<string>();
}