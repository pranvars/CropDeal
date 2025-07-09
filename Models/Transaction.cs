using System;
using System.Collections.Generic;

namespace TransactionService;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int DealerId { get; set; }

    public int FarmerId { get; set; }

    public int CropId { get; set; }

    public string CropName { get; set; } = null!;

    public int Quantity { get; set; }

    public double TotalAmount { get; set; }

    public bool TransactionStatus { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}
