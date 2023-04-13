namespace Yogeshwar.DB.DbModels;

/// <summary>
/// Class ProductImage. This class cannot be inherited.
/// </summary>
[Table("ProductImages")]
internal sealed class ProductImage
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
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    public string Image { get; set; }

    /// <summary>
    /// Gets or sets the product.
    /// </summary>
    /// <value>The product.</value>
    [ForeignKey(nameof(ProductId))] public Product Product { get; set; }
}