using System;
using System.Collections.Generic;

namespace Yogeshwar.DB.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ProductAccessories = new HashSet<ProductAccessory>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string ModelNo { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string Video { get; set; } = null!;
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductAccessory> ProductAccessories { get; set; }
    }
}
