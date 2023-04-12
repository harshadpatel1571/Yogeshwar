namespace Yogeshwar.DB.DbModels;

[Table("OrderDetails")]
internal sealed class OrderDetail
{
    [Key] public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Amount { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime? ReceiveDate { get; set; }

    public byte Status { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]

    public DateTime CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    [ForeignKey(nameof(OrderId))] public Order Order { get; set; }

    [ForeignKey(nameof(ProductId))] public Product Product { get; set; }
}