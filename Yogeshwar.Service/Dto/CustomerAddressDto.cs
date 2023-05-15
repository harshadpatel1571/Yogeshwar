namespace Yogeshwar.Service.Dto;

/// <summary>
/// Class CustomerAddressDto.
/// </summary>
public sealed class CustomerAddressDto
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    /// <value>The customer identifier.</value>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    /// <value>The city.</value>
    [Required(ErrorMessage = "City is required.")]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "City must be 3 to 50 character long.")]
    public string City { get; set; }

    /// <summary>
    /// Gets or sets the district.
    /// </summary>
    /// <value>The district.</value>
    [Required(ErrorMessage = "District is required.")]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "District must be 3 to 50 character long.")]
    public string District { get; set; }

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    /// <value>The state.</value>
    [Required(ErrorMessage = "State is required.")]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "State must be 3 to 50 character long.")]
    public string State { get; set; }

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    /// <value>The address.</value>
    [Required(ErrorMessage = "Address is required.")]
    [StringLength(250, MinimumLength = 10, ErrorMessage = "Address must be 10 to 250 character long.")]
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the pin code.
    /// </summary>
    /// <value>The pin code.</value>
    [Required(ErrorMessage = "PinCode is required.")]
    [StringLength(7, MinimumLength = 5, ErrorMessage = "PinCode must be 5 to 7 character long.")]
    public string PinCode { get; set; }

    /// <summary>
    /// Gets or sets the phone no.
    /// </summary>
    /// <value>The phone no.</value>
    [StringLength(13, MinimumLength = 10, ErrorMessage = "Phone no must be 10 to 13 character long.")]
    public string? PhoneNo { get; set; }
}