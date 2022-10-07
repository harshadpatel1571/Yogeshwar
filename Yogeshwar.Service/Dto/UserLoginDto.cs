namespace Yogeshwar.Service.Dto;

public sealed class UserLoginDto
{
    [Required(ErrorMessage = "User name is required.")]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "User name must be 8 to 25 character long.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "Password must be 8 to 25 character long.")]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}