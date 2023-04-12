namespace Yogeshwar.DB.DbModels;

[Table("ProductAccessories")]
internal sealed class ProductAccessory
{
    [Key] public int Id { get; set; }

    public int ProductId { get; set; }

    public int AccessoriesId { get; set; }

    public int Quantity { get; set; }

    [ForeignKey(nameof(AccessoriesId))] public Accessory Accessories { get; set; }

    [ForeignKey(nameof(ProductId))] public Product Product { get; set; }

    public IList<Notification> Notifications { get; } = new List<Notification>();
}