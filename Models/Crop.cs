using System;
using System.Collections.Generic;

namespace CropService;

public partial class Crop
{
    public int CropId { get; set; }

    public int FarmerId { get; set; }

    public string Name { get; set; } = null!;

    public double Quantity { get; set; }

    public double Price { get; set; }
}
