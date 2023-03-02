using System;
using System.Collections.Generic;

namespace Yogeshwar.DB.Models;

public partial class ProductAccessory
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int AccessoriesId { get; set; }

    public int Quantity { get; set; }

    public virtual Accessory Accessories { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual Product Product { get; set; } = null!;
}
