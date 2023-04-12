namespace Yogeshwar.DB.DbModels;

[Table(nameof(Order))]
internal sealed class Order
{
    [Key] public int Id { get; set; }

    public int CustomerId { get; set; }

    public decimal? Discount { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime OrderDate { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public Customer Customer { get; set; }

    public IList<CustomerService> CustomerServices { get; set; } = new List<CustomerService>();

    public IList<Notification> Notifications { get; set; } = new List<Notification>();

    public IList<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}