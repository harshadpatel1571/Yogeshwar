namespace Yogeshwar.DB.DbModels;

[Table(nameof(CustomerService))]
internal sealed class CustomerService
{
    [Key] public int Id { get; set; }

    [MaxLength(50)] [Unicode(false)] public string WorkerName { get; set; }

    public int OrderId { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime ComplainDate { get; set; }

    public byte Status { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime? ServiceCompletionDate { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(OrderId))] public Order Order { get; set; }
}