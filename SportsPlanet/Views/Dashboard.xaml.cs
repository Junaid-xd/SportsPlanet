using SportsPlanet.Helpers;
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

            UpdateCheckoutTotal();
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

            if(AddressBox.Text == "")
            {
                MessageBox.Show("Provide shipping address.");
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

                int placedOrderId = orderService.PlaceOrder(
                    AuthService.loggedInUser.Id,
                    CartService.CartItems.ToList(),
                    DeliveryTypeBox.Text,
                    PaymentBox.Text,
                    AddressBox.Text
                );

                // 4. Handle result
                if (placedOrderId != 0)
                {
                    // Clear cart
                    CartService.CartItems.Clear();

                    // Update UI
                    UpdateCartTotal();
                    CartColumn.Width = new GridLength(0);
                    CheckoutOverlay.Visibility = Visibility.Collapsed;

                    refreshPage();
                    var result = MessageBox.Show(
                                    "Order placed successfully!\n\nDo you want to download/print receipt?",
                                    "Order Success",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question
                                );

                    if (result == MessageBoxResult.Yes)
                    {
                        GenerateReceipt(placedOrderId);
                    }


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


        private void GenerateReceipt(int placedOrderId)
        {
            try
            {
                var orderService = new OrderService();

                // Fetch fresh order from DB
                var order = orderService.GetOrderWithProductsDetailsById(placedOrderId);

                if (order == null)
                {
                    MessageBox.Show("Order not found!");
                    return;
                }

                var user = AuthService.loggedInUser;

                var lines = order.OrderItems.Select(x => new ReceiptLine
                {
                    ProductName = x.Product?.ProductName ?? "Unknown",
                    Quantity = x.Quantity,
                    UnitPrice = x.Price,
                    TotalPrice = x.Price * x.Quantity
                }).ToList();

                double grandTotal = (double)order.TotalAmount;

                DateTime date =
                    DateTimeOffset.FromUnixTimeSeconds(order.CreatedAt).DateTime;

                string path = PdfGeneratorHelper.GenerateBill(
                    lines,
                    user,
                    grandTotal,
                    order.Id,
                    date
                );

                MessageBox.Show($"Receipt saved at:\n{path}");

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate receipt: " + ex.Message);
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

        private void ApplyFilter(string tag)
        {
            var list = productService.GetProductsByTag(tag);

            Products.Clear();

            foreach (var p in list)
                Products.Add(p);
        }
        private void FilterAllClick(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void FilterNewClick(object sender, RoutedEventArgs e)
        {
            ApplyFilter("new");
        }

        private void FilterBatsClick(object sender, RoutedEventArgs e)
        {
            ApplyFilter("bat");
        }

        private void FilterBallsClick(object sender, RoutedEventArgs e)
        {
            ApplyFilter("ball");
        }

        private void FilterBadmintonClick(object sender, RoutedEventArgs e)
        {
            ApplyFilter("badminton");
        }

        private void FilterKitsClick(object sender, RoutedEventArgs e)
        {
            ApplyFilter("kit");
        }

        private void UpdateCheckoutTotal()
        {
            if (CheckoutTotalText == null)
                return;

            decimal itemsTotal = CartService.CartItems
                .Sum(x => x.Product.Price * x.Quantity);

            decimal delivery = GetDeliveryCharges();

            decimal finalTotal = itemsTotal + delivery;

            CheckoutTotalText.Text = $"{finalTotal}";
        }

        private decimal GetDeliveryCharges()
        {
            if (DeliveryTypeBox.SelectedIndex == 0)
                return 300;

            if (DeliveryTypeBox.SelectedIndex == 1)
                return 500;

            return 0;
        }

        private void DeliveryTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCheckoutTotal();
        }
    }
}
