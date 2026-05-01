




using SportsPlanet.Helpers;
using SportsPlanet.Models;
using SportsPlanet.Services;
using System.Windows;
using System.Windows.Controls;

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

        private void CreateAdminClick(object sender, RoutedEventArgs e)
        {
            if (T1.Text == "" || T2.Text == "" || T3.Text == "" || T4.Text == "" || RoleBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            if (T3.Text != T4.Text)
            {
                MessageBox.Show("Passwords don't match");
                return;
            }

            string role = ((ComboBoxItem)RoleBox.SelectedItem).Content.ToString();

            bool result = CreateUser(T1.Text, T2.Text, T3.Text, role);

            if (result)
            {
                MessageBox.Show("Admin Created Successfully");
            }
            else
            {
                MessageBox.Show("Error creating admin");
            }
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
