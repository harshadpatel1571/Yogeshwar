namespace Yogeshwar.Service.Dto;

public class AccessoriesDto : BaseDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be 3 to 100 character long.")]
    public string Name { get; set; }

    [StringLength(int.MaxValue, MinimumLength = 5, ErrorMessage = "Description must be at least 5 character long.")]
    public string? Description { get; set; }

    public string? Image { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    public int Quantity { get; set; }

    public IFormFile? File { get; set; }
}