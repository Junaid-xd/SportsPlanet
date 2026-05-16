using SportsPlanet.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace SportsPlanet.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private Frame frame;
        private AuthService authService;

        public LoginPage(Frame fra)
        {
            InitializeComponent();
            frame = fra;
            authService = new AuthService();
        }


        private void LoginClick(object sender, RoutedEventArgs e)
        {
            // Reset errors
            EmailError.Text = "";
            PasswordError.Text = "";

            T1.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155"));
            T2.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155"));

            bool hasError = false;

            // Email validation
            if (string.IsNullOrWhiteSpace(T1.Text))
            {
                EmailError.Text = "Email is required";
                T1.BorderBrush = Brushes.Red;
                hasError = true;
            }

            // Password validation
            if (string.IsNullOrWhiteSpace(T2.Password))
            {
                PasswordError.Text = "Password is required";
                T2.BorderBrush = Brushes.Red;
                hasError = true;
            }

            // Stop if validation failed
            if (hasError)
                return;

            
            if (authService.login(T1.Text, T2.Password))
            {
                if (AuthService.loggedInUser.Role == "USER")
                {
                    frame.Navigate(new Dashboard(frame));
                }
                else if (AuthService.loggedInUser.Role == "ADMIN")
                {
                    frame.Navigate(new AdminManageProducts(frame));
                }
                else if (AuthService.loggedInUser.Role == "SUPER_ADMIN")
                {
                    frame.Navigate(new SuperAdminReports(frame));
                }
            }
            else
            {
                PasswordError.Text = "Incorrect credentials";
                T2.BorderBrush = Brushes.Red;
                T1.BorderBrush = Brushes.Red;
            }
        }

        private void goToSignupPage(Object sender, MouseButtonEventArgs e)
        {
            frame.Navigate(new SignupPage(frame));
        }

        public void goToDashboardPage(Object sender, MouseButtonEventArgs e) 
        {
            frame.Navigate(new Dashboard(frame));
        }
    }
}
