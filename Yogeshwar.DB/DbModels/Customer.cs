namespace Yogeshwar.DB.DbModels;

[Table(nameof(Customer))]
internal sealed class Customer
{
    [Key] public int Id { get; set; }

    [MaxLength(25)] public string FirstName { get; set; }

    [MaxLength(25)] public string LastName { get; set; }

    [DataType(DataType.EmailAddress)] public string? Email { get; set; }

    [DataType(DataType.PhoneNumber)] public string PhoneNo { get; set; }

    [MaxLength(15)] [Unicode(false)] public string? GstNumber { get; set; }

    [MaxLength(15)] [Unicode(false)] public string IfscCode { get; set; }

    [MaxLength(100)] [Unicode(false)] public string AccountHolderName { get; set; }

    [MaxLength(25)] [Unicode(false)] public string BankName { get; set; }

    [MaxLength(50)] [Unicode(false)] public string BranchName { get; set; }

    public long AccountNumber { get; set; }

    [MaxLength(50)] [Unicode(false)] public string? Image { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public IList<CustomerAddress> CustomerAddresses { get; } = new List<CustomerAddress>();

    public IList<Order> Orders { get; } = new List<Order>();
}