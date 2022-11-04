using System;
using System.Collections.Generic;

namespace Yogeshwar.Web.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public string ModelNo { get; set; } = null!;

    public string? Video { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual ICollection<ProductAccessory> ProductAccessories { get; } = new List<ProductAccessory>();

    public virtual ICollection<ProductImage> ProductImages { get; } = new List<ProductImage>();
}
