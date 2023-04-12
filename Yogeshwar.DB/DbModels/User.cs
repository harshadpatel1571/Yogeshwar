namespace Yogeshwar.DB.DbModels;

[Table("Users")]
internal sealed class User
{
    [Key] public int Id { get; set; }

    [Unicode(false)] [MaxLength(50)] public string Name { get; set; }

    [DataType(DataType.EmailAddress)] public string? Email { get; set; }

    [Unicode(false)] [MaxLength(25)] public string Username { get; set; }

    [DataType(DataType.PhoneNumber)] public string PhoneNo { get; set; }

    [DataType(DataType.Password)] public string Password { get; set; }

    public byte UserType { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = GlobalDataType.DateDataType)]

    public DateTime? ModifiedDate { get; set; }
}