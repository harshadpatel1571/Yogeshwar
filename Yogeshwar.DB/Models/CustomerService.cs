﻿using System;
using System.Collections.Generic;

namespace Yogeshwar.DB.Models;

public partial class CustomerService
{
    public int Id { get; set; }

    public string WorkerName { get; set; } = null!;

    public int OrderId { get; set; }

    public string? Description { get; set; }

    public DateTime ComplainDate { get; set; }

    public byte Status { get; set; }

    public DateTime? ServiceCompletedDate { get; set; }

    public virtual Order Order { get; set; } = null!;
}
