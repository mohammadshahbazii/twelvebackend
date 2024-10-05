using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class Acess
{
    public int AccessId { get; set; }

    public string AccessText { get; set; } = null!;

    public virtual ICollection<SelectedAdminAccess> SelectedAdminAccesses { get; set; } = new List<SelectedAdminAccess>();
}
