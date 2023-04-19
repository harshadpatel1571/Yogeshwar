namespace Yogeshwar.DB.DbModels;

/// <summary>
/// Class Configuration. This class cannot be inherited.
/// </summary>
[Table(nameof(Configuration))]
internal sealed class Configuration
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [Key] public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the company.
    /// </summary>
    /// <value>The name of the company.</value>
    [MaxLength(100)] public string CompanyName { get; set; }

    /// <summary>
    /// Gets or sets the company logo.
    /// </summary>
    /// <value>The company logo.</value>
    [Unicode(false)][MaxLength(50)] public string? CompanyLogo { get; set; }

    /// <summary>
    /// Gets or sets the GST number.
    /// </summary>
    /// <value>The GST number.</value>
    [Unicode(false)][MaxLength(15)] public string GstNumber { get; set; }

    /// <summary>
    /// Gets or sets the term and condition.
    /// </summary>
    /// <value>The term and condition.</value>
    [Unicode(false)][MaxLength(1000)] public string TermAndCondition { get; set; }

    /// <summary>
    /// Gets or sets the GST.
    /// </summary>
    /// <value>The GST.</value>
    public decimal Gst { get; set; }
}