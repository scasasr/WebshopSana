using System;
using System.Collections.Generic;

namespace WebShop.Domain.Models;

public partial class PayMethod
{
    public int PayMethodId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
