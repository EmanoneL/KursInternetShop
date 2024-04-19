using System;
using System.Collections.Generic;

namespace DB;

public partial class Cart
{
    public int IdCarts { get; set; }

    public int IdCustomers { get; set; }

    public int? Count { get; set; }

    public int? Cost { get; set; }

    public virtual Customer IdCustomersNavigation { get; set; }
}
