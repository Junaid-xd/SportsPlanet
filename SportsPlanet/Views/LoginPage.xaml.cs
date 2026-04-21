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
            if(T1.Text == "" || T2.Text == "")
            {
                MessageBox.Show("Please fill all fields");
                return;
            }
            else
            {
                
                if(authService.login(T1.Text, T2.Text))
                {
                    frame.Navigate(new Dashboard(frame));
                }
                else
                {
                    MessageBox.Show("Incorrect credentials");
                }
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
