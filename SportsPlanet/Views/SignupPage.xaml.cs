using SportsPlanet.Helpers;
using SportsPlanet.Models;
using SportsPlanet.Services;

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
            if(T1.Text == "" || T2.Text=="" || T3.Text == "" || T4.Text == "")
            {
                MessageBox.Show("Please fill all fields");
            }
            else
            {
                if (T3.Text != T4.Text) 
                {
                    MessageBox.Show("Passwords don't match");
                    return;
                }

                if(createNewUserFlow(T1.Text, T2.Text, T3.Text))
                {
                    MessageBox.Show("User Created");
                    frame.Navigate(new LoginPage(frame));
                }
                else
                {
                    MessageBox.Show("Error in creating user");
                }
                
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
    }
}
