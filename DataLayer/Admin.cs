using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class Admin
{
    public int AdminId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<SelectedAdminAccess> SelectedAdminAccesses { get; set; } = new List<SelectedAdminAccess>();
}
