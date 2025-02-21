using System;
using System.Collections.Generic;

namespace WebShop.Domain.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int PayMethodId { get; set; }

    public int PaymentStatusId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? PaidDate { get; set; }

    public decimal Total { get; set; }

    public virtual ICollection<OrderProductRequested> OrderProductRequesteds { get; set; } = new List<OrderProductRequested>();

    public virtual PayMethod PayMethod { get; set; } = null!;

    public virtual PaymentStatus PaymentStatus { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
