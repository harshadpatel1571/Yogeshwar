namespace Yogeshwar.Helper.Attribute;

internal sealed class ValidateFileAttribute : ValidationAttribute
{
    internal bool IsRequired { get; set; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return IsRequired
                ? new ValidationResult($"{validationContext.DisplayName} is required.")
                : ValidationResult.Success;
        }

        if (value is IFormFile file)
        {
            return ValidateFiles(validationContext, file);
        }
        else if (value is IEnumerable<IFormFile> files)
        {
            return ValidateFiles(validationContext, files.ToArray());
        }

        return ValidationResult.Success;
    }

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

file class FileValidationModel
{
    public int ImageSize { get; set; }

    public int VideoSize { get; set; }

    public string[] ImageExtensions { get; set; } = Array.Empty<string>();

    public string[] VideoExtensions { get; set; } = Array.Empty<string>();
}