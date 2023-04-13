namespace Yogeshwar.DB.DbModels;

/// <summary>
/// Class ProductCategory. This class cannot be inherited.
/// </summary>
[Table("ProductCategories")]
internal sealed class ProductCategory
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [Key] public int Id { get; set; }

    /// <summary>
    /// Gets or sets the product identifier.
    /// </summary>
    /// <value>The product identifier.</value>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the category identifier.
    /// </summary>
    /// <value>The category identifier.</value>
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the category.
    /// </summary>
    /// <value>The category.</value>
    [ForeignKey(nameof(CategoryId))] public Category Category { get; set; }

    /// <summary>
    /// Gets or sets the product.
    /// </summary>
    /// <value>The product.</value>
    [ForeignKey(nameof(ProductId))] public Product Product { get; set; }
}