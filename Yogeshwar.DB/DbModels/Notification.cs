namespace Yogeshwar.DB.DbModels;

[Table(nameof(Notification))]
internal sealed class Notification
{
    [Key] public int Id { get; set; }

    public int ProductAccessoriesId { get; set; }

    public int OrderId { get; set; }

    public bool IsCompleted { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime Date { get; set; }

    [ForeignKey(nameof(OrderId))] public Order Order { get; set; }

    [ForeignKey(nameof(ProductAccessoriesId))]
    public ProductAccessory ProductAccessories { get; set; }
}