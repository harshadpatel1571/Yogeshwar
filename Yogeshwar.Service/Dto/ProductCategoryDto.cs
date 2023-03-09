namespace Yogeshwar.Service.Dto;

public class ProductCategoryDto : BaseDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Product is required.")]
    public int ProductId { get; set; }
}