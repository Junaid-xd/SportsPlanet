using SportsPlanet.Models;
using SportsPlanet.Services;

using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Controls;


namespace SportsPlanet.Views
{
    public partial class Dashboard : Page
    {
        private Frame frame;
        private ProductService productService;
        private AuthService authService;
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        public Dashboard(Frame fra)
        {
            InitializeComponent();
            frame = fra;
            HeaderControl.SetFrame(frame);
            HeaderControl.SetActive("All Products");
            HeaderControl.OnFilterSelected += FilterProducts;

            productService = new ProductService();
            authService = new AuthService();

            LoadProducts();

            CartItemsControl.ItemsSource = CartService.CartItems;
            CheckoutItemsControl.ItemsSource = CartService.CartItems;
        }

        private void LoadProducts()
        {
            var list = productService.GetAllProducts();

            ProductsItemsControl.ItemsSource = null;

            Products.Clear();
            foreach (var p in list)
                Products.Add(p);
            ProductsItemsControl.ItemsSource = Products;
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            //UpdateAuthUI();

            CartItemsControl.ItemsSource = null;
            CartItemsControl.ItemsSource = CartService.CartItems;

            CheckoutItemsControl.ItemsSource = null;
            CheckoutItemsControl.ItemsSource = CartService.CartItems;

            // restore cart visibility
            if (CartService.CartItems.Count > 0)
            {
                CartColumn.Width = new GridLength(0.4, GridUnitType.Star);
            }
            else
            {
                CartColumn.Width = new GridLength(0);
            }

            // restore total
            UpdateCartTotal();
        }

        private void AddToCartClick(object sender, RoutedEventArgs e)
        {
            Button? btn = sender as Button;
            Product? product = btn.Tag as Product;

            if (product != null)
            {
                CartService.AddToCart(product);

                UpdateCartTotal();

                // Open cart on first item
                if (CartService.CartItems.Count == 1)
                {
                    CartColumn.Width = new GridLength(0.4, GridUnitType.Star);
                }
            }
        }

        private void RemoveFromCartClick(object sender, RoutedEventArgs e)
        {
            Button? btn = sender as Button;
            CartItem? item = btn.Tag as CartItem;

            if (item != null)
            {
                CartService.CartItems.Remove(item);
                UpdateCartTotal();
            }

            // Close cart if empty
            if (CartService.CartItems.Count == 0)
            {
                CartColumn.Width = new GridLength(0);
            }
        }

        private void UpdateCartTotal()
        {
            decimal total = CartService.CartItems.Sum(x => x.Product.Price * x.Quantity);
            TotalText.Text = $"Rs. {total}";
        }

        private void IncreaseQuantityClick(object sender, RoutedEventArgs e)
        {
            Button ?btn = sender as Button;
            CartItem ?item = btn.Tag as CartItem;

            if (item != null)
            {
                item.Quantity++;
                UpdateCartTotal();
            }
        }

        private void DecreaseQuantityClick(object sender, RoutedEventArgs e)
        {
            Button ?btn = sender as Button;
            CartItem ?item = btn.Tag as CartItem;

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    CartService.CartItems.Remove(item);
                }

                UpdateCartTotal();
            }

            if (CartService.CartItems.Count == 0)
            {
                CartColumn.Width = new GridLength(0);
            }
        }

        private void HandleCheckout(object sender, RoutedEventArgs e)
        {
            if (CartService.CartItems.Count == 0)
            {
                MessageBox.Show("Cart is empty!");
                return;
            }

            CheckoutOverlay.Visibility = Visibility.Visible;
        }

        private void CloseCheckout(object sender, RoutedEventArgs e)
        {
            CheckoutOverlay.Visibility = Visibility.Collapsed;
        }

        private void refreshPage()
        {
            frame.Navigate(new Dashboard(frame));
        }
        private void ConfirmOrderClick(object sender, RoutedEventArgs e)
        {
            // 1. Check login
            if (!AuthService.isLoggedIn)
            {
                frame.Navigate(new LoginPage(frame));
                return;
            }

            // 2. Check cart
            if (CartService.CartItems.Count == 0)
            {
                MessageBox.Show("Cart is empty!");
                return;
            }

            if (QuantityValidation())
            {
                // 3. Call OrderService
                var orderService = new OrderService();

                bool success = orderService.PlaceOrder(
                    AuthService.loggedInUser.Id,
                    CartService.CartItems.ToList(),
                    AddressBox.Text,
                    DeliveryTypeBox.Text,
                    PaymentBox.Text
                );

                // 4. Handle result
                if (success)
                {
                    // Clear cart
                    CartService.CartItems.Clear();

                    // Update UI
                    UpdateCartTotal();
                    CartColumn.Width = new GridLength(0);
                    CheckoutOverlay.Visibility = Visibility.Collapsed;

                    refreshPage();
                    MessageBox.Show("Order placed successfully!");


                }
                else
                {
                    MessageBox.Show("Failed to place order!");
                }
            }
            else
            {
                CheckoutOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private bool QuantityValidation()
        {
            foreach (var item in CartService.CartItems)
            {
                var product = productService.GetProductById(item.Product.Id);

                if (product == null)
                {
                    MessageBox.Show("Product not found.");
                    return false;
                }

                if (item.Quantity > product.Quantity)
                {
                    MessageBox.Show(
                        $"{product.ProductName} only has {product.Quantity} items available.",
                        "Stock Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return false;
                }
            }

            return true;
        }
        private void FilterProducts(string tag)
        {
            var list = productService.GetProductsByTag(tag);
            Products.Clear();
            foreach (var p in list)
                Products.Add(p);
        }
    }
}
