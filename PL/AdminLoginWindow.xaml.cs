
using PL.Engineer;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace PL;

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

    private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        PasswordBox passwordBox = sender as PasswordBox;
            adminPassword = passwordBox.Password;     // now adminPassword contains the password

    }
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        adminName = ((TextBox)sender).Text;  // now EngineerName contains the user name
    }

    private void UserLogin_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            //if (adminPassword == "8" && adminName == "e")//check if this is admin
           // {
                new AdminViewWindow().Show();
                Close();
            //}
            //else MessageBox.Show("User does not exist");
        }
        catch
        {
            MessageBox.Show("User does not exist");
        }

    }


    

}
