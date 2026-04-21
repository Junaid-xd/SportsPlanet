using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsPlanet.Models
{
    public class CartItem : INotifyPropertyChanged  //To Reflect Changes in UI instantly
    {
        public Product ?Product { get; set; }

        private int quantity = 1;

        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(TotalPrice)); // important
                }
            }
        }

        public decimal TotalPrice => Product.Price * Quantity;

        public event PropertyChangedEventHandler ?PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
