namespace Yogeshwar.DB.Models;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public decimal? Discount { get; set; }

    public DateTime OrderDate { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<CustomerService> CustomerServices { get; set; } = new List<CustomerService>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
