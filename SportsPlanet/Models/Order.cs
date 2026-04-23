using System;
using System.Collections.Generic;

namespace SportsPlanet.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public long CreatedAt { get; set; }

    public string? DeliveryType { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User User { get; set; } = null!;
}
