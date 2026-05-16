




using SportsPlanet.Helpers;
using SportsPlanet.Models;
using SportsPlanet.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SportsPlanet.Views
{
    public partial class SuperAdminCreateNewAdmin : Page
    {
        private AuthService authService;
        private Frame frame;

        public SuperAdminCreateNewAdmin(Frame fra )
        {
            InitializeComponent();
            frame = fra;
            HeaderControl.SetFrame(frame);
            HeaderControl.SetActive("Create New Admin");

            authService = new AuthService();
        }

        //private void CreateAdminClick(object sender, RoutedEventArgs e)
        //{
        //    if (T1.Text == "" || T2.Text == "" || T3.Text == "" || T4.Text == "" || RoleBox.SelectedItem == null)
        //    {
        //        MessageBox.Show("Please fill all fields");
        //        return;
        //    }

        //    if (T3.Text != T4.Text)
        //    {
        //        MessageBox.Show("Passwords don't match");
        //        return;
        //    }

        //    string role = ((ComboBoxItem)RoleBox.SelectedItem).Content.ToString();

        //    bool result = CreateUser(T1.Text, T2.Text, T3.Text, role);

        //    if (result)
        //    {
        //        MessageBox.Show("Admin Created Successfully");
        //    }
        //    else
        //    {
        //        MessageBox.Show("Error creating admin");
        //    }
        //}


        private void CreateAdminClick(object sender, RoutedEventArgs e)
        {
            // reset errors
            NameError.Text = "";
            EmailError.Text = "";
            PasswordError.Text = "";
            ConfirmPasswordError.Text = "";
            RoleError.Text = "";

            T1.BorderBrush = Brushes.Gray;
            T2.BorderBrush = Brushes.Gray;
            T3.BorderBrush = Brushes.Gray;
            T4.BorderBrush = Brushes.Gray;
            RoleBox.BorderBrush = Brushes.Gray;

            bool hasError = false;

            // Name
            if (string.IsNullOrWhiteSpace(T1.Text))
            {
                NameError.Text = "Full name is required";
                T1.BorderBrush = Brushes.Red;
                hasError = true;
            }

            // Email
            if (string.IsNullOrWhiteSpace(T2.Text))
            {
                EmailError.Text = "Email is required";
                T2.BorderBrush = Brushes.Red;
                hasError = true;
            }

            // Password
            if (string.IsNullOrWhiteSpace(T3.Text))
            {
                PasswordError.Text = "Password is required";
                T3.BorderBrush = Brushes.Red;
                hasError = true;
            }

            // Confirm Password
            if (string.IsNullOrWhiteSpace(T4.Text))
            {
                ConfirmPasswordError.Text = "Confirm password is required";
                T4.BorderBrush = Brushes.Red;
                hasError = true;
            }
            else if (T3.Text != T4.Text)
            {
                ConfirmPasswordError.Text = "Passwords don't match";
                T4.BorderBrush = Brushes.Red;
                hasError = true;
            }

            // Role
            if (RoleBox.SelectedItem == null)
            {
                RoleError.Text = "Please select a role";
                RoleBox.BorderBrush = Brushes.Red;
                hasError = true;
            }

            if (hasError) return;

            string role = ((ComboBoxItem)RoleBox.SelectedItem).Content.ToString();

            bool result = CreateUser(T1.Text, T2.Text, T3.Text, role);

            if (result)
            {
                ShowSuccess("Admin created successfully");
                NameError.Text = "";
                EmailError.Text = "";
                PasswordError.Text = "";
                ConfirmPasswordError.Text = "";
                RoleError.Text = "";

                T1.Text = "";
                T2.Text = "";
                T3.Text = "";
                T4.Text = "";   
            }
            else
            {
                ShowError("Email already registered");
            }
        }

        private async void ShowSuccess(string msg)
        {
            Toast.Background = new SolidColorBrush(Color.FromRgb(34, 197, 94));
            ToastText.Text = msg;
            Toast.Visibility = Visibility.Visible;

            await Task.Delay(2000);
            Toast.Visibility = Visibility.Collapsed;
        }

        private async void ShowError(string msg)
        {
            Toast.Background = new SolidColorBrush(Color.FromRgb(239, 68, 68));
            ToastText.Text = msg;
            Toast.Visibility = Visibility.Visible;

            await Task.Delay(2000);
            Toast.Visibility = Visibility.Collapsed;
        }

        private bool CreateUser(string name, string email, string password, string role)
        {
            User user = new User
            {
                Name = name,
                Email = email,
                Password = Security.HashPassword(password),
                Role = role,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            return authService.AddUser(user);
        }
    }
}
