namespace Yogeshwar.DB.DbModels;

[Table("Accessories")]
internal sealed class Accessory
{
    [Key] public int Id { get; set; }

    [MaxLength(100)] public string Name { get; set; }

    public string? Description { get; set; }

    [Unicode(false)] [MaxLength(50)] public string? Image { get; set; }

    public int Quantity { get; set; }

    [Unicode(false)] [MaxLength(10)] public string MeasurementType { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public IList<ProductAccessory> ProductAccessories { get; } = new List<ProductAccessory>();
}