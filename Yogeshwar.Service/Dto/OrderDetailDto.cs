namespace Yogeshwar.Service.Dto;

public class OrderDetailDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product is required.")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    public int Quantity { get; set; }

    public decimal Amount { get; set; }

    public string? ReceivedDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public OrderStatus Status { get; set; }
}