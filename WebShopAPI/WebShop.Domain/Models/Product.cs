using System;
using System.Collections.Generic;

namespace WebShop.Domain.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string ReferenceCode { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<OrderProductRequested> OrderProductRequesteds { get; set; } = new List<OrderProductRequested>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
