using System;
using System.Collections.Generic;

namespace Yogeshwar.Web
{
    public partial class Accessory
    {
        public Accessory()
        {
            ProductAccessories = new HashSet<ProductAccessory>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int Quantity { get; set; }

        public virtual ICollection<ProductAccessory> ProductAccessories { get; set; }
    }
}
