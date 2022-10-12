using System;
using System.Collections.Generic;

namespace Yogeshwar.Web
{
    public partial class ProductAccessory
    {
        public ProductAccessory()
        {
            Notifications = new HashSet<Notification>();
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int AccessoriesId { get; set; }
        public int Quantity { get; set; }

        public virtual Accessory Accessories { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
