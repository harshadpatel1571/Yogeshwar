using System;
using System.Collections.Generic;

namespace Yogeshwar.DB.Models;

public partial class Configuration
{
    public int Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? CompanyLogo { get; set; }

    public string Gstnumber { get; set; } = null!;

    public string TermAndCondition { get; set; } = null!;

    public decimal Gst { get; set; }
}
