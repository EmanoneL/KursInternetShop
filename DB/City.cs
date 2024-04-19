using System;
using System.Collections.Generic;

namespace DB;

public partial class City
{
    public int Id { get; set; }

    public string CityName { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
