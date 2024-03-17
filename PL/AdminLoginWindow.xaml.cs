using PL.Engineer;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace PL
{
    /// <summary>
    /// Interaction logic for AdminLoginWindow.xaml
    /// </summary>
    public partial class AdminLoginWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public AdminLoginWindow()
        {
            InitializeComponent();
        }

        private string adminName = string.Empty;
        private String adminPassword = string.Empty;

        // Event handler for when the password is changed in the password box.
        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            adminPassword = passwordBox.Password;     // Now adminPassword contains the password
        }

        // Event handler for when text is changed in the username TextBox.
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            adminName = ((TextBox)sender).Text;  // Now adminName contains the user name
        }

        // Event handler for the login button click.
        private void UserLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if the entered username and password match the admin credentials.
                //if (adminPassword == "8" && adminName == "e")
                //{
                //    // Open the AdminViewWindow.
                  new AdminViewWindow().Show();
                   Close();
                //}
                //else
                //{
                //    MessageBox.Show("User does not exist");
                //}
            }
            catch
            {
                MessageBox.Show("User does not exist");
            }
        }
    }
}
