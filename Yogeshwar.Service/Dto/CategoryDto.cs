namespace Yogeshwar.Service.Dto;

public class CategoryDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public IFormFile? ImageFile { get; set; }
}