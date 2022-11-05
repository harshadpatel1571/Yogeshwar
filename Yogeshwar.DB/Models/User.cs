namespace Yogeshwar.DB.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string Username { get; set; } = null!;

    public string PhoneNo { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte UserType { get; set; }

    public DateTime CreatedDate { get; set; }
}
