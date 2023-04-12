namespace Yogeshwar.DB.DbModels;

[Table(nameof(CustomerAddress))]
internal sealed class CustomerAddress
{
    [Key] public int Id { get; set; }

    public int CustomerId { get; set; }

    [MaxLength(50)] [Unicode(false)] public string City { get; set; }

    [MaxLength(50)] [Unicode(false)] public string District { get; set; }

    [MaxLength(50)] [Unicode(false)] public string State { get; set; }

    [MaxLength(250)] public string Address { get; set; }

    [MaxLength(7)] [Unicode(false)] public string PinCode { get; set; }

    [DataType(DataType.PhoneNumber)] public string? PhoneNo { get; set; }

    [ForeignKey(nameof(CustomerId))] public Customer Customer { get; set; }
}