using System;
using System.Collections.Generic;

namespace WebShop.Domain.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Identification { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public int RoleId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;
}
