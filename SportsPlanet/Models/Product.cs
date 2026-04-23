using System;
using System.Collections.Generic;

namespace SportsPlanet.Models;

public partial class Product
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string? ImgPath { get; set; }

    public string? Tags { get; set; }

    public DateTime? AvailabilityDate { get; set; }

    public long CreatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();
}
