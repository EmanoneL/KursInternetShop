using System;
using System.Collections.Generic;

namespace DB;

public partial class Product
{
    public int IdProducts { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public byte[] Picture { get; set; }

    public int Cost { get; set; }

    public int IdCategory { get; set; }

    public int IdSellers { get; set; }

    public int IdStorages { get; set; }

    public string Status { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Category IdCategoryNavigation { get; set; }

    public virtual Seller IdSellersNavigation { get; set; }

    public virtual Storage IdStoragesNavigation { get; set; }
}
