namespace Yogeshwar.DB.Models
{
    public partial class CustomerService
    {
        public int Id { get; set; }
        public string WorkerName { get; set; } = null!;
        public int CustomerId { get; set; }
        public string? Description { get; set; }
        public DateTime ComplainDate { get; set; }
        public byte Status { get; set; }
        public DateTime? ServiceCompletedDate { get; set; }

        public virtual Customer Customer { get; set; } = null!;
    }
}
