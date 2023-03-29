using System.Diagnostics;

namespace Yogeshwar.Service.Dto;

public class OrderDetailDto : BaseDto
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
}

public class CustomerViewDto
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public string? Email { get; set; }

    public required string PhoneNo { get; set; }

    public required string Address { get; set; }

    public string? Image { get; set; }
}

[DebuggerDisplay("Price is {Price}")]
public class OrderDetailsViewDto
{
    public string? Image { get; set; }
    
    public required string ProductName { get; set; }

    public required string Status { get; set; }

    public required string Price { get; set; }

    public required int Quantity { get; set; }

    public required string TotalAmount { get; set; }

    public required string DeliveredDate { get; set; }
}

public class OrderDetailViewModel
{
    public required string OrderId { get; set; }

    public required CustomerViewDto Customer { get; set; }

    public required IList<OrderDetailsViewDto> OrderDetails { get; set; }

    public required string SubTotal { get; set; }

    public required string Discount { get; set; }

    public required string Total { get; set; }
}