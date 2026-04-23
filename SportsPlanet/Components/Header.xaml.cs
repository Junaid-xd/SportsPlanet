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
        private Button? activeButton;
        public event Action<string>? OnFilterSelected;

        public Header()
        {
            InitializeComponent();
        }

        // IMPORTANT: Any file that uses header will pass frame here
        public void SetFrame(Frame frm)
        {
            frame = frm;
            UpdateAuthUI();
        }


        public void SetActive(string btnName)
        {
            Button? btn = btnName switch
            {
                "All Products" => AllProductsBtn,
                "New Arrivals" => NewArrivalBtn,
                "Bats" => BatsBtn,
                "Balls" => BallsBtn,
                "Badminton" => BadmintonBtn,
                "Kits" => KitsBtn,
                "Orders" => OrdersBtn,
                "Manage Products" => ManageProductsBtn,
                "Reports" => ReportsBtn,
                "Dispatch Orders" => DispatchOrdersBtn,
                _ => null
            };

            // if invalid name, just return
            if (btn == null) return;

            // reset previous button to original color scheme
            if (activeButton != null)
            {
                activeButton.Background = null;
                activeButton.Foreground = (Brush)Application.Current.Resources["SecondaryText"];
            }

            // set new active
            activeButton = btn;

            activeButton.Background = (Brush)Application.Current.Resources["PrimaryBlue"];
            activeButton.Foreground = (Brush)Application.Current.Resources["OnPrimaryText"];
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
            frame.Navigate(new OrdersView(frame));
        }

        private void UpdateAuthUI()
        {

            if (AuthService.isLoggedIn && AuthService.loggedInUser.Role == "USER")
            {
                ShowUserUi();
            }
            else if(AuthService.isLoggedIn && AuthService.loggedInUser.Role == "ADMIN")
            {
                ShowAdminUi();
            }
            else if(AuthService.isLoggedIn && AuthService.loggedInUser.Role == "SUPER_ADMIN")
            {
                ShowSuperAdminUI();
            }
            else
            {
                ShowGeneralUi();
            }
        }

        private void ShowProductsFilterUI()
        {
            NewArrivalBtn.Visibility = Visibility.Visible;
            BatsBtn.Visibility = Visibility.Visible;
            BallsBtn.Visibility = Visibility.Visible;
            BadmintonBtn.Visibility = Visibility.Visible;
            KitsBtn.Visibility = Visibility.Visible;
        }

        private void ShowGeneralUi()
        {
            ShowProductsFilterUI();
            AllProductsBtn.Visibility = Visibility.Visible;
            loginBtn.Visibility = Visibility.Visible;
            signupBtn.Visibility = Visibility.Visible;

            //SetActive(AllProductsBtn);
        }

        private void ShowUserUi()
        {
            ShowProductsFilterUI();

            AllProductsBtn.Visibility = Visibility.Visible;
            OrdersBtn.Visibility = Visibility.Visible;
            logoutBtn.Visibility = Visibility.Visible;

            //SetActive(AllProductsBtn);
        }

        private void ShowAdminUi()
        {
            ManageProductsBtn.Visibility = Visibility.Visible;
            DispatchOrdersBtn.Visibility = Visibility.Visible;
            logoutBtn.Visibility = Visibility.Visible;

            //SetActive(ManageProductsBtn);
        }

        private void ShowSuperAdminUI()
        {
            ReportsBtn.Visibility = Visibility.Visible;
            CreateNewAdminAndSuperAdminBtn.Visibility = Visibility.Visible;
            logoutBtn.Visibility = Visibility.Visible;

            //SetActive(ReportsBtn);

        }

        private void HandleNewArrivalClick(object sender, RoutedEventArgs e)
        {
            SetActive("New Arrivals");
            OnFilterSelected?.Invoke("new");
        }


        private void HandleBatsClick(object sender, RoutedEventArgs e)
        {
            SetActive("Bats");
            OnFilterSelected?.Invoke("bat");
        }

        private void HandleBallClick(object sender, RoutedEventArgs e)
        {
            SetActive("Balls");
            OnFilterSelected?.Invoke("ball");
        }

        private void HandleBadmintonClick(object sender, RoutedEventArgs e)
        {
            SetActive("Badminton");
            OnFilterSelected?.Invoke("badminton");
        }

        private void HandleKitsClick(object sender, RoutedEventArgs e)
        {
            SetActive("Kits");
            OnFilterSelected?.Invoke("kit");
        }


        private void HandleAllProductsClick(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Dashboard(frame));
        }

    }
}
