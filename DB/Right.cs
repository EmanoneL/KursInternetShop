using System;
using System.Collections.Generic;

namespace DB;

public partial class Right
{
    public int? UserId { get; set; }

    public int? MenuItemId { get; set; }

    public int? Rd { get; set; }

    public int? Write { get; set; }

    public int? Edit { get; set; }

    public int? Del { get; set; }

    public virtual Menu MenuItem { get; set; }

    public virtual User User { get; set; }
}
