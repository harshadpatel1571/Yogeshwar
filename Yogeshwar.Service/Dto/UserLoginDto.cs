namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class UserLoginDto. This class cannot be inherited.
/// Implements the <see cref="BaseDto" />
/// </summary>
/// <seealso cref="BaseDto" />
public sealed class UserLoginDto : BaseDto
{
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    [Required(ErrorMessage = "User name is required.")]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "User name must be 8 to 25 character long.")]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>The password.</value>
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "Password must be 8 to 25 character long.")]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [remember me].
    /// </summary>
    /// <value><c>true</c> if [remember me]; otherwise, <c>false</c>.</value>
    public bool RememberMe { get; set; }
}