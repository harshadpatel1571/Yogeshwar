namespace Yogeshwar.Service.Dto;

public class CategoryDto : BaseDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be 3 to 50 character long.")]
    public string Name { get; set; }

    public string? Image { get; set; }

    public IFormFile? ImageFile { get; set; }
}