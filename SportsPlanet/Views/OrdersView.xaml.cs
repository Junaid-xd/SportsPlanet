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

                if (orders.Count == 0)
                {
                    MessageBox.Show("No orders found!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
            }
        }
    }
}
