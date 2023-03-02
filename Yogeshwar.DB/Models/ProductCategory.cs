using System;
using System.Collections.Generic;

namespace Yogeshwar.DB.Models;

public partial class ProductCategory
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int CategoryId { get; set; }
}
