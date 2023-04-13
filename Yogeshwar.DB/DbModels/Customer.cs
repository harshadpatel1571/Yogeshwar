namespace Yogeshwar.DB.DbModels;

/// <summary>
/// Class Customer. This class cannot be inherited.
/// </summary>
[Table(nameof(Customer))]
internal sealed class Customer
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [Key] public int Id { get; set; }

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    /// <value>The first name.</value>
    [MaxLength(25)] public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    /// <value>The last name.</value>
    [MaxLength(25)] public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [DataType(DataType.EmailAddress)] public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the phone no.
    /// </summary>
    /// <value>The phone no.</value>
    [DataType(DataType.PhoneNumber)] public string PhoneNo { get; set; }

    /// <summary>
    /// Gets or sets the GST number.
    /// </summary>
    /// <value>The GST number.</value>
    [MaxLength(15)] [Unicode(false)] public string? GstNumber { get; set; }

    /// <summary>
    /// Gets or sets the ifsc code.
    /// </summary>
    /// <value>The ifsc code.</value>
    [MaxLength(15)] [Unicode(false)] public string IfscCode { get; set; }

    /// <summary>
    /// Gets or sets the name of the account holder.
    /// </summary>
    /// <value>The name of the account holder.</value>
    [MaxLength(100)] [Unicode(false)] public string AccountHolderName { get; set; }

    /// <summary>
    /// Gets or sets the name of the bank.
    /// </summary>
    /// <value>The name of the bank.</value>
    [MaxLength(25)] [Unicode(false)] public string BankName { get; set; }

    /// <summary>
    /// Gets or sets the name of the branch.
    /// </summary>
    /// <value>The name of the branch.</value>
    [MaxLength(50)] [Unicode(false)] public string BranchName { get; set; }

    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    /// <value>The account number.</value>
    public long AccountNumber { get; set; }

    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    [MaxLength(50)] [Unicode(false)] public string? Image { get; set; }

    /// <summary>
    /// Gets or sets the created date.
    /// </summary>
    /// <value>The created date.</value>
    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the created by.
    /// </summary>
    /// <value>The created by.</value>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the modified date.
    /// </summary>
    /// <value>The modified date.</value>
    [Column(TypeName = GlobalDataType.DateDataType)]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets the modified by.
    /// </summary>
    /// <value>The modified by.</value>
    public int? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is deleted.
    /// </summary>
    /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets the customer addresses.
    /// </summary>
    /// <value>The customer addresses.</value>
    public IList<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();

    /// <summary>
    /// Gets the orders.
    /// </summary>
    /// <value>The orders.</value>
    public IList<Order> Orders { get; set; } = new List<Order>();
}