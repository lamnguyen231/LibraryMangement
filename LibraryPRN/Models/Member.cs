using System;
using System.Collections.Generic;

namespace LibraryPRN.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string? Name { get; set; }

    public DateOnly? JoinDate { get; set; }

    public string? Email { get; set; }

    public bool RoleId { get; set; }

    public string? Passwords { get; set; }

    public virtual ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>();

    public String GetRole()
    {
        return RoleId == true ? "Admin" : "User";
    }
}
