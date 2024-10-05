using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class SelectedAdminAccess
{
    public int SaaId { get; set; }

    public int AdminId { get; set; }

    public int AccessId { get; set; }

    public virtual Acess Access { get; set; } = null!;

    public virtual Admin Admin { get; set; } = null!;
}
