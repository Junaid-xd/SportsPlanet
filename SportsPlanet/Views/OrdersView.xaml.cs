using SportsPlanet.Helpers;
using SportsPlanet.Models;
using SportsPlanet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SportsPlanet.Views
{
    /// <summary>
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    public partial class OrdersView : Page
    {
        private Frame frame;
        private OrderService orderService;

        public OrdersView(Frame fra)
        {
            InitializeComponent();
            frame = fra;
            HeaderControl.SetFrame(frame);
            HeaderControl.SetActive("Orders");
            orderService = new OrderService();

            Loaded += PageLoaded; // same pattern as Dashboard
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            if (!AuthService.isLoggedIn)
            {
                MessageBox.Show("Please login first!");
                frame.Navigate(new LoginPage(frame));
                return;
            }

            try
            {
                var orders = orderService.GetUserOrders(AuthService.loggedInUser.Id);

                OrdersItemsControl.ItemsSource = null;
                OrdersItemsControl.ItemsSource = orders;

                //if (orders.Count == 0)
                //{
                //    MessageBox.Show("No orders found!");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
            }
        }

        private void PrintReceiptClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                int orderId = (int)btn.Tag;

                var result = MessageBox.Show(
                    "Do you want to generate receipt for this order?",
                    "Print Receipt",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    GenerateReceipt(orderId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void GenerateReceipt(int orderId)
        {
            try
            {
                var order = orderService.GetOrderWithProductsDetailsById(orderId);

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

                DateTime date = DateTimeOffset.FromUnixTimeSeconds(order.CreatedAt).DateTime;

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
    }
}
