//using System;
//using System.Collections.Generic;
//using System.ComponentModel;

//namespace SportsPlanet.Models;

//public partial class Product: INotifyPropertyChanged
//{
//    public int Id { get; set; }

//    public string ProductName { get; set; } = null!;

//    public decimal Price { get; set; }

//    private int quantity;

//    public int Quantity
//    {
//        get => quantity;
//        set
//        {
//            if (quantity != value)
//            {
//                quantity = value;
//                OnPropertyChanged(nameof(Quantity));
//            }
//        }
//    }

//    public string? ImgPath { get; set; }

//    public string? Tags { get; set; }

//    public DateTime? AvailabilityDate { get; set; }

//    public long CreatedAt { get; set; }

//    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();

//    public event PropertyChangedEventHandler? PropertyChanged;

//    protected void OnPropertyChanged(string name)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
//    }
//}


















using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SportsPlanet.Models
{
    public partial class Product : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string productName = null!;
        public string ProductName
        {
            get => productName;
            set
            {
                if (productName != value)
                {
                    productName = value;
                    OnPropertyChanged(nameof(ProductName));
                }
            }
        }

        private decimal price;
        public decimal Price
        {
            get => price;
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

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

        private string? imgPath;
        public string? ImgPath
        {
            get => imgPath;
            set
            {
                if (imgPath != value)
                {
                    imgPath = value;
                    OnPropertyChanged(nameof(ImgPath));
                }
            }
        }

        private string? tags;
        public string? Tags
        {
            get => tags;
            set
            {
                if (tags != value)
                {
                    tags = value;
                    OnPropertyChanged(nameof(Tags));
                }
            }
        }

        public DateTime? AvailabilityDate { get; set; }

        public long CreatedAt { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // ================= NOTIFICATION =================
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
