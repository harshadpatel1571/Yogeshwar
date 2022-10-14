namespace Yogeshwar.Service.Dto;

public class AccessoriesDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name must be up to 100 character.")]
    public string Name { get; set; }

    [StringLength(250, MinimumLength = 3, ErrorMessage = "Description must be up to 250 character.")]
    public string? Description { get; set; }

    public string? Image { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    public int Quantity { get; set; }

    public IFormFile? File { get; set; }
}