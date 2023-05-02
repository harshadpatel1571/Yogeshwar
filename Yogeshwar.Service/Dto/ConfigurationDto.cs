namespace Yogeshwar.Service.Dto;

public sealed class ConfigurationDto : BaseDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Company Name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Company Name must be 3 to 100 character long.")]
    public string CompanyName { get; set; }

    public string? CompanyLogo { get; set; }

    [Required(ErrorMessage = "Gst Number is required.")]
    [StringLength(15, MinimumLength = 3, ErrorMessage = "Gst Number must be 15 character long.")]
    public string GstNumber { get; set; }

    [Required(ErrorMessage = "Term and Condition is required.")]
    [StringLength(1000, ErrorMessage = "Term and Condition maximum 1000 character long.")]
    public string TermAndCondition { get; set; }

    public decimal Gst { get; set; }

    [ValidateFile]
    public IFormFile? ImageFile { get; set; }
}
