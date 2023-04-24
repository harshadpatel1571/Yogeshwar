namespace Yogeshwar.DB.DbModels;

/// <summary>
/// Class Product. This class cannot be inherited.
/// </summary>
[Table("Products")]
internal sealed class Product
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
    [MaxLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>The price.</value>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the model no.
    /// </summary>
    /// <value>The model no.</value>
    [MaxLength(50)]
    public string ModelNo { get; set; }

    /// <summary>
    /// Gets or sets the HSN no.
    /// </summary>
    /// <value>The HSN no.</value>
    [Unicode(false)]
    [MaxLength(10)]
    public string HsnNo { get; set; }

    /// <summary>
    /// Gets or sets the GST.
    /// </summary>
    /// <value>The GST.</value>
    public decimal Gst { get; set; }

    /// <summary>
    /// Gets or sets the video.
    /// </summary>
    /// <value>The video.</value>
    [Unicode(false)]
    [MaxLength(10)]
    public string? Video { get; set; }

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
    /// Gets or sets the order details.
    /// </summary>
    /// <value>The order details.</value>
    public IList<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    /// <summary>
    /// Gets or sets the product accessories.
    /// </summary>
    /// <value>The product accessories.</value>
    public IList<ProductAccessory> ProductAccessories { get; set; } = new List<ProductAccessory>();

    /// <summary>
    /// Gets or sets the product categories.
    /// </summary>
    /// <value>The product categories.</value>
    public IList<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    /// <summary>
    /// Gets or sets the product images.
    /// </summary>
    /// <value>The product images.</value>
    public IList<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}