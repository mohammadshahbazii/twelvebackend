using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class Order
{
    public int OrderId { get; set; }

    public int Amount { get; set; }

    public DateTime CreateDate { get; set; }

    public bool IsFinally { get; set; }
}
