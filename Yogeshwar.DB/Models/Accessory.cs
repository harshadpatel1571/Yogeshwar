using System;
using System.Collections.Generic;

namespace Yogeshwar.DB.Models;

public partial class Accessory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Image { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ProductAccessory> ProductAccessories { get; } = new List<ProductAccessory>();
}
