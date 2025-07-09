using System;
using System.Collections.Generic;

namespace DealerService;

public partial class Dealer
{
    public int DealerId { get; set; }

    public int UserId { get; set; }

    public string Location { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

    public string BankIfsccode { get; set; } = null!;

    public bool IsActive { get; set; } = true;
}
