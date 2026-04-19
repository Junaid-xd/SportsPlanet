using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportsPlanet.Models;

[Table("users")]
[Index("Email", Name = "UQ__users__AB6E61640E0967C2", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("password")]
    [StringLength(255)]
    public string Password { get; set; } = null!;

    [Column("role")]
    [StringLength(20)]
    public string Role { get; set; } = null!;

    [Column("created_at")]
    public long CreatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
