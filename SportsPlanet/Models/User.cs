using System;
using System.Collections.Generic;

namespace SportsPlanet.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public long CreatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
