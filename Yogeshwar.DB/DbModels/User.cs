namespace Yogeshwar.DB.DbModels;

/// <summary>
/// Class User. This class cannot be inherited.
/// </summary>
[Table("Users")]
internal sealed class User
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [Key] public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [Unicode(false)][MaxLength(50)] public string Name { get; set; }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [MaxLength(100)][Unicode(false)] public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>The username.</value>
    [Unicode(false)][MaxLength(25)] public string Username { get; set; }

    /// <summary>
    /// Gets or sets the phone no.
    /// </summary>
    /// <value>The phone no.</value>
    [DataType(DataType.PhoneNumber)] public string PhoneNo { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>The password.</value>
    [MaxLength(250)][Unicode(false)] public string Password { get; set; }

    /// <summary>
    /// Gets or sets the type of the user.
    /// </summary>
    /// <value>The type of the user.</value>
    public byte UserType { get; set; }

    /// <summary>
    /// Gets or sets the created date.
    /// </summary>
    /// <value>The created date.</value>
    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the modified date.
    /// </summary>
    /// <value>The modified date.</value>
    [Column(TypeName = GlobalDataType.DateDataType)]

    public DateTime? ModifiedDate { get; set; }
}