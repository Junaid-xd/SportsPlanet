
using SportsPlanet.Models;
using SportsPlanet.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SportsPlanet.Views
{
    public partial class AdminDispatchedOrders : Page
    {
        private Frame frame;
        private OrderService orderService;

        public ObservableCollection<Order> Orders { get; set; }
            = new ObservableCollection<Order>();

        public AdminDispatchedOrders(Frame fra)
        {
            InitializeComponent();
            frame = fra;

            HeaderControl.SetFrame(frame);
            HeaderControl.SetActive("Dispatched Orders");

            orderService = new OrderService();

            LoadOrders();
        }

        private void LoadOrders()
        {
            Orders.Clear();

            var list = orderService.GetOrdersByStatus("DISPATCHED");

            foreach (var o in list)
                Orders.Add(o);

            OrdersItemsControl.ItemsSource = Orders;

            FilterStatusText.Text = "Showing all orders";
        }

        // ================= FILTER =================
        private void FilterClick(object sender, RoutedEventArgs e)
        {
            var from = FromDatePicker.SelectedDate;
            var to = ToDatePicker.SelectedDate;

            // If no filter selected → show all
            if (from == null && to == null)
            {
                LoadOrders();
                return;
            }

            var filtered = orderService
                .GetOrdersByStatus("DISPATCHED")
                .Where(o =>
                {
                    var date = DateTimeOffset
                        .FromUnixTimeSeconds(o.CreatedAt)
                        .Date;

                    if (from != null && date < from.Value.Date)
                        return false;

                    if (to != null && date > to.Value.Date)
                        return false;

                    return true;
                });

            Orders.Clear();

            foreach (var o in filtered)
                Orders.Add(o);

            // Update status text
            string text = "Filtered: ";

            if (from != null)
                text += $"From {from.Value:dd MMM yyyy} ";

            if (to != null)
                text += $"To {to.Value:dd MMM yyyy}";

            FilterStatusText.Text = text;
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

        private void ResetFilterClick(object sender, RoutedEventArgs e)
        {
            FromDatePicker.SelectedDate = null;
            ToDatePicker.SelectedDate = null;

            LoadOrders();
        }
    }
}