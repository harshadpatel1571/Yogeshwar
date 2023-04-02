namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class CustomerDto.
/// Implements the <see cref="BaseDto" />
/// </summary>
/// <seealso cref="BaseDto" />
public class CustomerDto : BaseDto
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    /// <value>The first name.</value>
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "First name must be 3 to 25 character long.")]
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    /// <value>The last name.</value>
    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "Last name must be 3 to 25 character long.")]
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [EmailAddress(ErrorMessage = "Value must be email address.")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Email must be 5 to 50 character long.")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the phone no.
    /// </summary>
    /// <value>The phone no.</value>
    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(12, MinimumLength = 10, ErrorMessage = "Phone number must be 10 to 12 character long.")]
    public string PhoneNo { get; set; }

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    /// <value>The address.</value>
    [Required(ErrorMessage = "Address is required.")]
    [StringLength(250, MinimumLength = 5, ErrorMessage = "Address must be 5 to 250 character long.")]
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    /// <value>The city.</value>
    [Required(ErrorMessage = "City is required.")]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "City must be 3 to 25 character long.")]
    public string City { get; set; }

    /// <summary>
    /// Gets or sets the pin code.
    /// </summary>
    /// <value>The pin code.</value>
    [Required(ErrorMessage = "Pin code is required.")]
    public int PinCode { get; set; }

    /// <summary>
    /// Gets or sets the GST number.
    /// </summary>
    /// <value>The GST number.</value>
    [StringLength(15, MinimumLength = 15, ErrorMessage = "GST number must be 15 character long.")]
    public string? GstNumber { get; set; }

    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    /// <value>The account number.</value>
    [Required(ErrorMessage = "Account number is required.")]
    public long AccountNumber { get; set; }

    /// <summary>
    /// Gets or sets the name of the account holder.
    /// </summary>
    /// <value>The name of the account holder.</value>
    [Required(ErrorMessage = "Account holder name is required.")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Account holder name must be 5 to 50 character long.")]
    public string AccountHolderName { get; set; }

    /// <summary>
    /// Gets or sets the name of the bank.
    /// </summary>
    /// <value>The name of the bank.</value>
    [Required(ErrorMessage = "Bank name is required.")]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "Bank name must be 3 to 25 character long.")]
    public string BankName { get; set; }

    /// <summary>
    /// Gets or sets the ifsc code.
    /// </summary>
    /// <value>The ifsc code.</value>
    [Required(ErrorMessage = "IFSCCode is required.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "IFSCCode must be 11 character long.")]
    public string IFSCCode { get; set; }

    /// <summary>
    /// Gets or sets the name of the branch.
    /// </summary>
    /// <value>The name of the branch.</value>
    [Required(ErrorMessage = "Branch name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Branch name must be 3 to 50 character long.")]
    public string BranchName { get; set; }

    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets the image file.
    /// </summary>
    /// <value>The image file.</value>
    [ValidateFile]
    public IFormFile? ImageFile { get; set; }
}