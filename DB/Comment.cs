namespace DB;

public partial class Comment
{
    public int IdComments { get; set; }

    public int IdProduct { get; set; }

    public int IdCustomers { get; set; }

    public int? Rating { get; set; }

    public string Description { get; set; }

    public string Date { get; set; }

    public virtual Customer IdCustomersNavigation { get; set; }

    public virtual Product IdProductNavigation { get; set; }
}
