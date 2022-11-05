﻿namespace Yogeshwar.DB.Models;

public partial class Accessory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Image { get; set; }

    public int Quantity { get; set; }

    public virtual ICollection<ProductAccessory> ProductAccessories { get; } = new List<ProductAccessory>();
}
