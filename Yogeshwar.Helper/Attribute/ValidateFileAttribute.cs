namespace Yogeshwar.Helper.Attribute;

internal sealed class ValidateFileAttribute : ValidationAttribute
{
    internal bool IsRequired { get; set; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return IsRequired
                ? new ValidationResult($"{validationContext.DisplayName} is required")
                : ValidationResult.Success;
        }

        if (value is not IFormFile file || value is not IEnumerable<IFormFile> files)
        {
            return ValidationResult.Success;
        }

        var fileValidationProperties = validationContext
            .GetService<IConfiguration>()
            .GetSection("Files")
            .Get<FileValidationModel>();

        return file is null
              ? ValidateFiles(fileValidationProperties, files.ToArray())
              : ValidateFiles(fileValidationProperties, file);
    }

    private static ValidationResult? ValidateFiles(FileValidationModel fileValidationProperties, params IFormFile[] files)
    {
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
                return new ValidationResult($"File up to size of {maxSize} MB is valid.");
        }

        return ValidationResult.Success;
    }

    private class FileValidationModel
    {
        public int ImageSize { get; set; }

        public int VideoSize { get; set; }

        public string[] ImageExtensions { get; set; } = Array.Empty<string>();

        public string[] VideoExtensions { get; set; } = Array.Empty<string>();
    }
}