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
            ProductImages = new HashSet<ProductImage>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string ModelNo { get; set; } = null!;
        public string? Video { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductAccessory> ProductAccessories { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
