using System.Collections.Generic;

namespace DB;

public partial class Category
{
    public int IdCategory { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
