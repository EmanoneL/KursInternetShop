using System;
using System.Collections.Generic;

namespace DB;

public partial class Address
{
    public int IdAddress { get; set; }

    public int City { get; set; }

    public int Street { get; set; }

    public string HomeNumber { get; set; }

    public virtual City CityNavigation { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Seller> Sellers { get; set; } = new List<Seller>();

    public virtual ICollection<Storage> Storages { get; set; } = new List<Storage>();

    public virtual Street StreetNavigation { get; set; }
}
