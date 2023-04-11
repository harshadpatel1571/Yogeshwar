using System;
using System.Collections.Generic;

namespace Yogeshwar.DB.Models;

public partial class CustomerAddress
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string City { get; set; } = null!;

    public string District { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PinCode { get; set; } = null!;

    public string? PhoneNo { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
