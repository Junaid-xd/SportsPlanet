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
            productService = new ProductService();
            authService = new AuthService();

            // Bind products directly to ListView (or ItemsControl) in XAML
            LoadProducts();
            ProductsItemsControl.ItemsSource = Products;

            CartItemsControl.ItemsSource = CartService.CartItems;
        }

        private void LoadProducts()
        {
            var list = productService.GetAllProducts();
            Products.Clear();
            foreach (var p in list)
                Products.Add(p);
        }

        public void LoginClick(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new LoginPage(frame));
        }

        public void SignupClick(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new SignupPage(frame));
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            UpdateAuthUI();
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            AuthService.logout();
            frame.Navigate(new LoginPage(frame));
        }

        private void UpdateAuthUI()
        {
            if (AuthService.isLoggedIn)
            {
                loginBtn.Visibility = Visibility.Collapsed;
                signupBtn.Visibility = Visibility.Collapsed;
                logoutBtn.Visibility = Visibility.Visible;
            }
            else
            {
                loginBtn.Visibility = Visibility.Visible;
                signupBtn.Visibility = Visibility.Visible;
                logoutBtn.Visibility = Visibility.Collapsed;
            }
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
    }
}
