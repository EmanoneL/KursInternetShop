using System.Collections.Generic;

namespace DB;

public partial class Cart
{
    public int IdCarts { get; set; }

    public int IdCustomers { get; set; }

    public int? Count { get; set; }

    public int? Cost { get; set; }

    public virtual Customer IdCustomersNavigation { get; set; }

    public List<Product> Products { get; set; } = new();
    public virtual ICollection<CartsHasProsucts> CartsHasProducts { get; set; }
}
