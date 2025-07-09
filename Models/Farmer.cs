using System;
using System.Collections.Generic;

namespace FarmerService;

public partial class Farmer
{
    public int FarmerId { get; set; }

    public int UserId { get; set; }

    public string Location { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

    public string BankIfsccode { get; set; } = null!;

    public bool IsActive { get; set; } = true;
}
