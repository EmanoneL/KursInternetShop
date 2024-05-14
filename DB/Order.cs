namespace DB;

public partial class Order
{
    public int IdOrders { get; set; }

    public int? IdDeliveryCompany { get; set; }

    public int IdCustomer { get; set; }

    public int IdProducts { get; set; }

    public int ProductCount { get; set; }

    public int TotalCost { get; set; }

    public string Status { get; set; }

    public int IdAddress { get; set; }

    public string RegistrationDate { get; set; }

    public virtual Address IdAddressNavigation { get; set; }

    public virtual Customer IdCustomerNavigation { get; set; }

    public virtual DeliveryCompany IdDeliveryCompanyNavigation { get; set; }

    public virtual Product IdProductsNavigation { get; set; }
}
