namespace Yogeshwar.Service.Dto;

public class CustomerDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Value must be email address.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Phone no is required.")]
    public string PhoneNo { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; }

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; }

    [Required(ErrorMessage = "Pincode is required.")]
    public int Pincode { get; set; }

    public int CreatedBy { get; set; }

    public int ModifiedBy { get; set; }
}
