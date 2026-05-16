
using SportsPlanet.Models;
using SportsPlanet.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SportsPlanet.Views
{
    public partial class AdminPendingOrders : Page
    {
        private Frame frame;
        private OrderService orderService;

        public ObservableCollection<Order> Orders { get; set; }
            = new ObservableCollection<Order>();

        public AdminPendingOrders(Frame fra)
        {
            InitializeComponent();
            frame = fra;

            HeaderControl.SetFrame(frame);
            HeaderControl.SetActive("Pending Orders");

            orderService = new OrderService();

            LoadPendingOrders();
        }

        private void LoadPendingOrders()
        {
            Orders.Clear();

            var list = orderService.GetOrdersByStatus("PENDING");

            foreach (var o in list)
                Orders.Add(o);

            OrdersItemsControl.ItemsSource = Orders;
        }

        // ================= VIEW DETAILS =================
        private void ViewDetailsClick(object sender, RoutedEventArgs e)
        {
            var order = (sender as Button)?.Tag as Order;
            if (order == null) return;

            CustomerText.Text = order.User.Email;
            AddressText.Text = order.Address;
            DeliveryText.Text = order.DeliveryType;
            TotalText.Text = $"{order.TotalAmount} Rs";

            DetailsItemsControl.ItemsSource = order.OrderItems;

            DetailsOverlay.Visibility = Visibility.Visible;
        }

        private void CloseDetails(object sender, RoutedEventArgs e)
        {
            DetailsOverlay.Visibility = Visibility.Collapsed;
        }

        private void DispatchOrderClick(object sender, RoutedEventArgs e)
        {
            var order = (sender as Button)?.Tag as Order;

            if (order == null) return;

            bool success = orderService.UpdateOrderStatus(order.Id, "DISPATCHED");

            if (success)
            {
                Orders.Remove(order);
                ShowToast("Order dispatched successfully!");
            }
            else
            {
                ShowToast("Failed to dispatch order!", false);
            }
        }

        private async void ShowToast(string message, bool isSuccess = true)
        {
            ToastText.Text = message;

            Toast.Background = isSuccess
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#16A34A"))
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));

            Toast.Visibility = Visibility.Visible;

            await Task.Delay(2500);

            Toast.Visibility = Visibility.Collapsed;
        }
    }
}