namespace Yogeshwar.Service.Dto;

public class ServiceDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Worker name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be between 3 to 50 character.")]
    public string WorkerName { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(250, ErrorMessage = "Discription must be upto 250 character.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Order is required.")]
    public int OrderId { get; set; }

    public string? OrderCustomerName { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public byte ServiceStatus { get; set; }

    public string? ServiceStatusString { get; set; }

    public string? ComplainDate { get; set; }

    public DateTime? CompletedDate { get; set; }
}
