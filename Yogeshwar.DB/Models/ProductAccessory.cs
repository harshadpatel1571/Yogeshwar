using System;
using System.Collections.Generic;

namespace Yogeshwar.DB.Models
{
    public partial class ProductAccessory
    {
        public ProductAccessory()
        {
            Notifications = new HashSet<Notification>();
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Quantity { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
