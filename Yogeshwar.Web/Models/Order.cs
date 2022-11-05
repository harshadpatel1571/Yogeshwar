namespace Yogeshwar.Web.Models;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public decimal? Discount { get; set; }

    public DateTime OrderDate { get; set; }

    public bool IsCompleted { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<CustomerService> CustomerServices { get; } = new List<CustomerService>();

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
