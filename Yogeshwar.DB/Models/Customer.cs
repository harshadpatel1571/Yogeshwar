using System;
using System.Collections.Generic;

namespace Yogeshwar.DB.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string PhoneNo { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? Gstnumber { get; set; }

    public string Ifsccode { get; set; } = null!;

    public string AccountHolderName { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string BranchName { get; set; } = null!;

    public long AccountNumber { get; set; }

    public int PinCode { get; set; }

    public string? Image { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
