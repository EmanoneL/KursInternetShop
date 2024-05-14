using System.Collections.Generic;

namespace DB;

public partial class Street
{
    public int Id { get; set; }

    public string StreetName { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
