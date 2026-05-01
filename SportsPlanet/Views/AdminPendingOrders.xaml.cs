
using SportsPlanet.Models;
using SportsPlanet.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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

            DetailsInfoText.Text =
                $"Customer: {order.User.Email}\n" +
                $"Address: {order.Address}\n" +
                $"Delivery: {order.DeliveryType}\n" +
                $"Total: Rs. {order.TotalAmount}";

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
                Orders.Remove(order); // instant UI update
                MessageBox.Show("Order dispatched!");
            }
            else
            {
                MessageBox.Show("Failed to dispatch order!");
            }
        }
    }
}