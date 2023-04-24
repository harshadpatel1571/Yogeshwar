namespace Yogeshwar.DB.DbModels;

/// <summary>
/// Class Category. This class cannot be inherited.
/// </summary>
[Table("Categories")]
internal sealed class Category
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [MaxLength(50)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the HSN no.
    /// </summary>
    /// <value>The HSN no.</value>
    [MaxLength(10)]
    [Unicode(false)]
    public string HsnNo { get; set; }

    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    [MaxLength(100)]
    [Unicode(false)]
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets the created date.
    /// </summary>
    /// <value>The created date.</value>
    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the created by.
    /// </summary>
    /// <value>The created by.</value>
    public int CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the modified date.
    /// </summary>
    /// <value>The modified date.</value>
    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets the modified by.
    /// </summary>
    /// <value>The modified by.</value>
    public int? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is deleted.
    /// </summary>
    /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets the product categories.
    /// </summary>
    /// <value>The product categories.</value>
    public IList<ProductCategory> ProductCategories { get; } = new List<ProductCategory>();
}