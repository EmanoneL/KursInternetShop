namespace DB;

public partial class Menu
{
    public int MenuItemId { get; set; }

    public int? ParentId { get; set; }

    public string ItemName { get; set; }

    public string Dll { get; set; }

    public string Function { get; set; }

    public int? Orders { get; set; }
}
