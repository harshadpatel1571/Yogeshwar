namespace Yogeshwar.Service.Dto;

public class OrderDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Customer is required.")]

    public int CustomerId { get; set; }

    public decimal? Discount { get; set; }

    public decimal Amount { get; set; }

    public string? CustomerName { get; set; }

    [Required(ErrorMessage = "Order date is required.")]
    public string OrderDate { get; set; }

    public bool IsCompleted { get; set; }

    public int OrderCount { get; set; }

    [Required(ErrorMessage = "Order details are required.")]
    public IList<OrderDetailDto> OrderDetail { get; set; }
}