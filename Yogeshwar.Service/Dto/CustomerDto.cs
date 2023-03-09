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
}