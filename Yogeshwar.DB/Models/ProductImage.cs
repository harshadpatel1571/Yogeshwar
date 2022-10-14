using System;
using System.Collections.Generic;

namespace Yogeshwar.DB.Models
{
    public partial class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Image { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
    }
}
