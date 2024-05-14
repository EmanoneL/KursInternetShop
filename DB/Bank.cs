using System.Collections.Generic;

namespace DB;

public partial class Bank
{
    public int IdBank { get; set; }

    public string Name { get; set; }

    public string PhoneNumer { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Seller> Sellers { get; set; } = new List<Seller>();
}
