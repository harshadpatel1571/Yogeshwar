namespace Yogeshwar.Service.Dto;

public class CustomerDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be 3 to 50 character.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be 3 to 50 character.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Value must be email address.")]
    [StringLength(100, ErrorMessage = "Email must be less than 100 character.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Phone no is required.")]
    [StringLength(12, MinimumLength = 10, ErrorMessage = "Phone number must be 10 to 12 character.")]
    public string PhoneNo { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(250, MinimumLength = 5, ErrorMessage = "Address must be 5 to 250 character.")]
    public string Address { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(25, MinimumLength = 5, ErrorMessage = "City must be 5 to 25 character.")]
    public string City { get; set; }

    [Required(ErrorMessage = "Pincode is required.")]
    public int Pincode { get; set; }

    public int CreatedBy { get; set; }

    public int ModifiedBy { get; set; }
}
