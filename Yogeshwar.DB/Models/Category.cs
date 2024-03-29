﻿namespace Yogeshwar.DB.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ProductCategory> ProductCategories { get; } = new List<ProductCategory>();
}
