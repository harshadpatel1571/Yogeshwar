namespace Yogeshwar.DB.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerServices = new HashSet<CustomerService>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public string PhoneNo { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Pincode { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual ICollection<CustomerService> CustomerServices { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
