using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SportsPlanet.Models;

public partial class Product: INotifyPropertyChanged
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    private int quantity;

    public int Quantity
    {
        get => quantity;
        set
        {
            if (quantity != value)
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
    }

    public string? ImgPath { get; set; }

    public string? Tags { get; set; }

    public DateTime? AvailabilityDate { get; set; }

    public long CreatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
