using SportsPlanet.Helpers;
using SportsPlanet.Models;
using SportsPlanet.Services;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;


namespace SportsPlanet.Views
{


    public partial class SignupPage : Page
    {
        private Frame frame;
        private AuthService authService;

        public SignupPage(Frame fra)
        {
            InitializeComponent();
            frame = fra;
            authService = new AuthService();
        }

        private void SignupButtonClick(object sender, RoutedEventArgs e)
        {
            string name = T1.Text.Trim();
            string email = T2.Text.Trim();
            string password = T3.Text;
            string confirmPassword = T4.Text;

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format");
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords don't match");
                return;
            }

            if (createNewUserFlow(name, email, password))
            {
                MessageBox.Show("User Created");
                frame.Navigate(new LoginPage(frame));
            }
            else
            {
                MessageBox.Show("Email already exists");
            }
        }

        private bool createNewUserFlow(String name, String email, String password)
        {
            User user = new User();
            user.Name = name;
            user.Email = email;
            user.Password = Security.HashPassword(password);
            user.Role = "USER";
            user.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return authService.AddUser(user);
        }

        private void GoToLoginPage(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new LoginPage(frame));
        }

        private void GoToDashboard(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Dashboard(frame));
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);
        }

    }
}
