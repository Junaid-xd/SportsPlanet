using SportsPlanet.Helpers;
using SportsPlanet.Models;
using SportsPlanet.Services;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;


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

        //private void SignupButtonClick(object sender, RoutedEventArgs e)
        //{
        //    string name = T1.Text.Trim();
        //    string email = T2.Text.Trim();
        //    string password = T3.Text;
        //    string confirmPassword = T4.Text;

        //    if (string.IsNullOrWhiteSpace(name) ||
        //        string.IsNullOrWhiteSpace(email) ||
        //        string.IsNullOrWhiteSpace(password) ||
        //        string.IsNullOrWhiteSpace(confirmPassword))
        //    {
        //        MessageBox.Show("Please fill all fields");
        //        return;
        //    }

        //    if (!IsValidEmail(email))
        //    {
        //        MessageBox.Show("Invalid email format");
        //        return;
        //    }

        //    if (password.Length < 6)
        //    {
        //        MessageBox.Show("Password must be at least 6 characters");
        //        return;
        //    }

        //    if (password != confirmPassword)
        //    {
        //        MessageBox.Show("Passwords don't match");
        //        return;
        //    }

        //    if (createNewUserFlow(name, email, password))
        //    {
        //        MessageBox.Show("User Created");
        //        frame.Navigate(new LoginPage(frame));
        //    }
        //    else
        //    {
        //        MessageBox.Show("Email already exists");
        //    }
        //}


        private void SignupButtonClick(object sender, RoutedEventArgs e)
        {
            string name = T1.Text.Trim();
            string email = T2.Text.Trim();
            string password = T3.Text;
            string confirmPassword = T4.Text;

            // Reset errors
            NameError.Text = "";
            EmailError.Text = "";
            PasswordError.Text = "";
            ConfirmPasswordError.Text = "";

            Brush defaultBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155"));

            T1.BorderBrush = defaultBrush;
            T2.BorderBrush = defaultBrush;
            T3.BorderBrush = defaultBrush;
            T4.BorderBrush = defaultBrush;

            bool hasError = false;

            // Name validation
            if (string.IsNullOrWhiteSpace(name))
            {
                NameError.Text = "Full name is required";
                T1.BorderBrush = Brushes.Red;
                hasError = true;
            }

            // Email validation
            if (string.IsNullOrWhiteSpace(email))
            {
                EmailError.Text = "Email is required";
                T2.BorderBrush = Brushes.Red;
                hasError = true;
            }
            else if (!IsValidEmail(email))
            {
                EmailError.Text = "Invalid email format";
                T2.BorderBrush = Brushes.Red;
                hasError = true;
            }

            // Password validation
            if (string.IsNullOrWhiteSpace(password))
            {
                PasswordError.Text = "Password is required";
                T3.BorderBrush = Brushes.Red;
                hasError = true;
            }
            else if (password.Length < 6)
            {
                PasswordError.Text = "Password must be at least 6 characters";
                T3.BorderBrush = Brushes.Red;
                hasError = true;
            }

            // Confirm password validation
            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                ConfirmPasswordError.Text = "Confirm password is required";
                T4.BorderBrush = Brushes.Red;
                hasError = true;
            }
            else if (password != confirmPassword)
            {
                ConfirmPasswordError.Text = "Passwords don't match";
                T4.BorderBrush = Brushes.Red;
                hasError = true;
            }

            // Stop execution if validation fails
            if (hasError)
                return;

            // Existing business logic untouched
            if (createNewUserFlow(name, email, password))
            {
                SuccessBox.Visibility = Visibility.Visible;
                SuccessText.Text = "Account created successfully! Redirecting to login...";

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(2);

                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    frame.Navigate(new LoginPage(frame));
                };

                timer.Start();
            }
            else
            {
                EmailError.Text = "Email already exists";
                T2.BorderBrush = Brushes.Red;
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
