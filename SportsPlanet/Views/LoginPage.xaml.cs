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

        public LoginPage(Frame fra)
        {
            InitializeComponent();
            frame = fra;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // after login success

            frame.Navigate(new Dashboard(frame));
        }

        private void goToSignupPage(Object sender, MouseButtonEventArgs e)
        {
            frame.Navigate(new SignupPage(frame));
        }
    }
}
