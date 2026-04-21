using SportsPlanet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsPlanet.Services
{
    internal class CartService
    {
        private static ObservableCollection<CartItem> cartItems = new ObservableCollection<CartItem>();

        public static ObservableCollection<CartItem> CartItems => cartItems;

        public static void AddToCart(Product product)
        {
            var existingItem = cartItems.FirstOrDefault(x => x.Product.Id == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cartItems.Add(new CartItem
                {
                    Product = product,
                    Quantity = 1
                });
            }
        }

        public static void RemoveFromCart(Product product)
        {
            var item = cartItems.FirstOrDefault(x => x.Product.Id == product.Id);

            if (item != null)
            {
                cartItems.Remove(item);
            }
        }
    }
}
