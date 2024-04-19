using System;
using System.Collections.Generic;

namespace DB;

public class StorageDisplayItem
{
    public int Id { get; set; }
    public string AddressString { get; set; }
}

public partial class Storage
{
    public int IdStorages { get; set; }

    public int IdAddress { get; set; }

    public virtual Address IdAddressNavigation { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
