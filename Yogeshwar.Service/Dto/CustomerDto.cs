namespace Yogeshwar.Service.Dto;

public class CustomerDto : BaseDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "First name must be 3 to 25 character long.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "Last name must be 3 to 25 character long.")]
    public string LastName { get; set; }

    [EmailAddress(ErrorMessage = "Value must be email address.")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Email must be 5 to 50 character long.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(12, MinimumLength = 10, ErrorMessage = "Phone number must be 10 to 12 character long.")]
    public string PhoneNo { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(250, MinimumLength = 5, ErrorMessage = "Address must be 5 to 250 character long.")]
    public string Address { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "City must be 3 to 25 character long.")]
    public string City { get; set; }

    [Required(ErrorMessage = "Pin code is required.")]
    public int PinCode { get; set; }

    [StringLength(15, MinimumLength = 15, ErrorMessage = "GST number must be 15 character long.")]
    public string? GstNumber { get; set; }

    [Required(ErrorMessage = "Account number is required.")]
    public long AccountNumber { get; set; }

    [Required(ErrorMessage = "Account holder name is required.")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Account holder name must be 5 to 50 character long.")]
    public string AccountHolderName { get; set; }

    [Required(ErrorMessage = "Bank name is required.")]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "Bank name must be 3 to 25 character long.")]
    public string BankName { get; set; }

    [Required(ErrorMessage = "IFSCCode is required.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "IFSCCode must be 11 character long.")]
    public string IFSCCode { get; set; }

    [Required(ErrorMessage = "Branch name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Branch name must be 3 to 50 character long.")]
    public string BranchName { get; set; }

    public string? Image { get; set; }

    [ValidateFile]
    public IFormFile? ImageFile { get; set; }
}