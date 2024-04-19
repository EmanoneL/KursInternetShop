using System;
using System.Collections.Generic;

namespace DB;

public partial class CartsHasProsuct
{
    public int IdCarts { get; set; }

    public int IdProducts { get; set; }

    public virtual Cart IdCartsNavigation { get; set; }

    public virtual Product IdProductsNavigation { get; set; }
}
