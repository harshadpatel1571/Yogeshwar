namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class UserDetailDto. This class cannot be inherited.
/// Implements the <see cref="BaseDto" />
/// </summary>
/// <seealso cref="BaseDto" />
public sealed class UserDetailDto : BaseDto
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>The username.</value>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the phone no.
    /// </summary>
    /// <value>The phone no.</value>
    public string PhoneNo { get; set; }

    /// <summary>
    /// Gets or sets the type of the user.
    /// </summary>
    /// <value>The type of the user.</value>
    public byte UserType { get; set; }
}