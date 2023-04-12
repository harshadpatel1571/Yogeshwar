namespace Yogeshwar.DB.DbModels;

[Table("ProductCategories")]
internal sealed class ProductCategory
{
    [Key] public int Id { get; set; }

    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))] public Category Category { get; set; }

    [ForeignKey(nameof(ProductId))] public Product Product { get; set; }
}