using SportsPlanet.Services;
using SportsPlanet.Views;
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

namespace SportsPlanet.Components
{
    public partial class Header : UserControl
    {
        private Frame? frame;

        public Header()
        {
            InitializeComponent();
        }

        // IMPORTANT: Dashboard will pass frame here
        public void SetFrame(Frame frm)
        {
            frame = frm;
            UpdateAuthUI();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new LoginPage(frame));
        }

        private void SignupClick(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new SignupPage(frame));
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            AuthService.logout();
            frame.Navigate(new LoginPage(frame));
        }

        private void GoToOrdersPage(object sender, RoutedEventArgs e)
        {
            if (!AuthService.isLoggedIn)
            {
                frame.Navigate(new LoginPage(frame));
                return;
            }

            frame.Navigate(new OrdersView(frame));
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


    }
}
