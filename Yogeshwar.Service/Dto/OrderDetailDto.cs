namespace Yogeshwar.Service.Dto;

public class OrderDetailDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product is required.")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    public int Quantity { get; set; }

    public decimal Amount { get; set; }

    public string? deliveredDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public OrderDetailStatus? Status { get; set; }

    public IList<AccessoriesOrderDto>? Accessories { get; set; }
}

public class AccessoriesOrderDto
{
    [Required(ErrorMessage = "Accessory id is required.")]
    public int Id { get; set; }

    public bool IsSelected { get; set; }
}