using System;
using System.Collections.Generic;

namespace WebShop.Domain.Models;

public partial class PaymentStatus
{
    public int PaymentStatusId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
