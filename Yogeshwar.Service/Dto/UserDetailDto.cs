namespace Yogeshwar.Service.Dto;

public sealed class UserDetailDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Email { get; set; }

    public string Username { get; set; }

    public string PhoneNo { get; set; }

    public byte UserType { get; set; }

    public string CreatedDate { get; set; }
}