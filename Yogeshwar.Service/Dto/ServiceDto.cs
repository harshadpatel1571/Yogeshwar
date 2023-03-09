namespace Yogeshwar.Service.Dto;

public class ServiceDto : BaseDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Worker name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Worker name must be 3 to 50 character long.")]
    public string WorkerName { get; set; }

    [StringLength(int.MaxValue, MinimumLength = 5, ErrorMessage = "Description must be at least 5 character long.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Order is required.")]
    public int OrderId { get; set; }

    public string? OrderCustomerName { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public byte ServiceStatus { get; set; }

    public string? ServiceStatusString { get; set; }

    public string? ComplainDate { get; set; }

    public DateTime? CompletedDate { get; set; }
}
