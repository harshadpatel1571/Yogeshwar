namespace Yogeshwar.DB.DbModels;

[Table("ProductImages")]
internal sealed class ProductImage
{
    [Key] public int Id { get; set; }

    public int ProductId { get; set; }

    public string Image { get; set; }

    [ForeignKey(nameof(ProductId))] public Product Product { get; set; }
}