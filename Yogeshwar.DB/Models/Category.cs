using System;
using System.Collections.Generic;

namespace Yogeshwar.DB.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public bool IsDeleted { get; set; }
}
