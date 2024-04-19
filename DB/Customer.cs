using System;
using System.Collections.Generic;

namespace DB;

public partial class Customer
{
    public int IdCustomers { get; set; }

    public int IdBank { get; set; }

    public string Name { get; set; }

    public byte[] Email { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public long PhoneNumber { get; set; }

    public int IdAddress { get; set; }

    public string Status { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Address IdAddressNavigation { get; set; }

    public virtual Bank IdBankNavigation { get; set; }
}
