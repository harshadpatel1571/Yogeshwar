namespace Yogeshwar.Service.Dto;

public class ProductDto : BaseDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be 3 to 100 character long.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(int.MaxValue, MinimumLength = 5, ErrorMessage = "Description must be at least 5 character long.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "Model no is required.")]
    [StringLength(50, ErrorMessage = "Model no must be up to 50 character long.")]
    public string ModelNo { get; set; }

    [Required(ErrorMessage = "Accessories are required.")]
    public IList<int> Accessories { get; set; }

    [Required(ErrorMessage = "Categories are required.")]
    public IList<int> Categories { get; set; }

    [Required(ErrorMessage = "Accessories quantities are required.")]
    public IList<AccessoriesQuantity> AccessoriesQuantity { get; set; }

    internal IList<ImageIds>? Images { get; set; }

    public SelectList? SelectListsForAccessories { get; set; }

    public SelectList? SelectListsForCategories { get; set; }

    public string? Video { get; set; }

    public IList<IFormFile>? ImageFiles { get; set; }

    public IFormFile? VideoFile { get; set; }
}

public class AccessoriesQuantity
{
    public int AccessoriesId { get; set; }

    public int Quantity { get; set; }

    internal string? Image { get; set; }
}

internal class ImageIds
{
    public int Id { get; set; }

    public string Image { get; set; }
}

public class ProductAccessoriesDto
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public int Quantity { get; set; }
}

internal class ProductAccessoriesDetailDto
{
    public string? Image { get; set; }

    public IList<AccessoriesDetailDto> Accessories { get; set; }

    public decimal Amount { get; set; }
}

public record struct AccessoriesDetailDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Image { get; set; }
}