namespace Yogeshwar.DB.DbModels;

[Table("Categories")]
internal sealed class Category
{
    [Key] public int Id { get; set; }

    [MaxLength(50)] public string Name { get; set; }

    [Unicode(false)] [MaxLength(10)] public string HsnNo { get; set; }

    [Unicode(false)] [MaxLength(50)] public string? Image { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public IList<ProductCategory> ProductCategories { get; } = new List<ProductCategory>();
}