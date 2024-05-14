namespace DB;

public partial class CartsHasProsucts
{
    public int IdCarts { get; set; }

    public int IdProducts { get; set; }

    public virtual Cart IdCartsNavigation { get; set; }

    public virtual Product IdProductsNavigation { get; set; }
}
