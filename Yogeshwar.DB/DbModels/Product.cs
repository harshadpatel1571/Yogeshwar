namespace Yogeshwar.DB.DbModels;

[Table("Products")]
internal sealed class Product
{
    [Key] public int Id { get; set; }

    [MaxLength(100)] public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    [MaxLength(50)] public string ModelNo { get; set; }

    [Unicode(false)] [MaxLength(10)] public string HsnNo { get; set; }

    public decimal Gst { get; set; }

    [Unicode(false)] [MaxLength(10)] public string? Video { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public IList<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public IList<ProductAccessory> ProductAccessories { get; set; } = new List<ProductAccessory>();

    public IList<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public IList<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}