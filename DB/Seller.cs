using System;
using System.Collections.Generic;

namespace DB;

public partial class Seller
{
    public int IdSellers { get; set; }

    public int IdBank { get; set; }

    public string Name { get; set; }

    public byte[] Logo { get; set; }

    public string Email { get; set; }

    public int PhoneNumber { get; set; }

    public int IdAddress { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public virtual Address IdAddressNavigation { get; set; }

    public virtual Bank IdBankNavigation { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
