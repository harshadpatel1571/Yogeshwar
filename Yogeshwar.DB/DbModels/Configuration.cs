namespace Yogeshwar.DB.DbModels;

[Table(nameof(Configuration))]
internal sealed class Configuration
{
    [Key] public int Id { get; set; }

    [MaxLength(100)] public string CompanyName { get; set; }

    [Unicode(false)] [MaxLength(50)] public string? CompanyLogo { get; set; }

    [Unicode(false)] [MaxLength(15)] public string GstNumber { get; set; }

    [Unicode(false)] [MaxLength(1000)] public string TermAndCondition { get; set; }

    public decimal Gst { get; set; }
}