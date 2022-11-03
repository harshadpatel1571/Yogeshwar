using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yogeshwar.Service.Dto
{
    public class ServiceDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Worker name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be 3 to 50 character.")]
        public string WorkerName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be 3 to 50 character.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Customer are required.")]
        public int CustomerId { get; set; }

        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Status are required.")]
        public byte ServiceStatus { get; set; }

        public string? ServiceStatusString { get; set; }

        public string? ComplainDate { get; set; }

        public DateTime? CompletedDate { get; set; }
    }
}
