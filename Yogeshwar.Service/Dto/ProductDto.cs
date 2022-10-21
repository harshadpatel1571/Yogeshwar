using Microsoft.AspNetCore.Mvc.Rendering;

namespace Yogeshwar.Service.Dto;

public class ProductDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name must be up to 100 character.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(250, ErrorMessage = "Name must be up to 250 character.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "Model no is required.")]
    [StringLength(50, ErrorMessage = "Name must be up to 50 character.")]
    public string ModelNo { get; set; }

    [Required(ErrorMessage = "Accessories are required.")]
    public IList<int> Accessories { get; set; }

    [Required(ErrorMessage = "Accessories quantities are required.")]
    public IList<AccessoriesQuantity> AccessoriesQuantity { get; set; }

    internal IList<ImageIds>? Images { get; set; }

    public SelectList? SelectListsForAccessories { get; set; }

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