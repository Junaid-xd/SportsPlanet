using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SportsPlanet.Models;

[Table("products")]
public partial class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_name")]
    [StringLength(150)]
    public string ProductName { get; set; } = null!;

    [Column("price", TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("img_path")]
    [StringLength(255)]
    public string? ImgPath { get; set; }

    [Column("tags")]
    [StringLength(500)]
    public string? Tags { get; set; }

    [Column("availability_date", TypeName = "date")]
    public DateTime? AvailabilityDate { get; set; }

    [Column("created_at")]
    public long CreatedAt { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();


    public string ImageSource
    {
        get
        {
            return new Uri(ImgPath, UriKind.Absolute).ToString();
        }
    }
}
