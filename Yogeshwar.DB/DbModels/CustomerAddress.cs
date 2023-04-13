namespace Yogeshwar.DB.DbModels;

/// <summary>
/// Class CustomerAddress. This class cannot be inherited.
/// </summary>
[Table(nameof(CustomerAddress))]
internal sealed class CustomerAddress
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [Key] public int Id { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    /// <value>The customer identifier.</value>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    /// <value>The city.</value>
    [MaxLength(50)] [Unicode(false)] public string City { get; set; }

    /// <summary>
    /// Gets or sets the district.
    /// </summary>
    /// <value>The district.</value>
    [MaxLength(50)] [Unicode(false)] public string District { get; set; }

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    /// <value>The state.</value>
    [MaxLength(50)] [Unicode(false)] public string State { get; set; }

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    /// <value>The address.</value>
    [MaxLength(250)] public string Address { get; set; }

    /// <summary>
    /// Gets or sets the pin code.
    /// </summary>
    /// <value>The pin code.</value>
    [MaxLength(7)] [Unicode(false)] public string PinCode { get; set; }

    /// <summary>
    /// Gets or sets the phone no.
    /// </summary>
    /// <value>The phone no.</value>
    [DataType(DataType.PhoneNumber)] public string? PhoneNo { get; set; }

    /// <summary>
    /// Gets or sets the customer.
    /// </summary>
    /// <value>The customer.</value>
    [ForeignKey(nameof(CustomerId))] public Customer Customer { get; set; }
}