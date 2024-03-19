using PL.Engineer;

using System.Windows;
using System.Windows.Controls;

namespace PL;

/// <summary>
/// Interaction logic for EngineerLoginWindow.xaml
/// </summary>
public partial class EngineerLoginWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // Variables to store user input
    private string EngineerName = "Dani Levi";//string.Empty;;
    private int id = 324567891;//0;

    public EngineerLoginWindow()
    {
        InitializeComponent();
    }


    private void TextBox_UserName(object sender, TextChangedEventArgs e)
    {
        // Retrieve the user name entered in the TextBox
        //EngineerName = ((TextBox)sender).Text;
    }


    private void TextBox_Id(object sender, TextChangedEventArgs e)
    {
        //TextBox textBox = sender as TextBox;
        //// Parse the entered text as an integer and store it in the id variable
        //if (int.TryParse(textBox.Text, out int result))
        //    id = result;
        //else 
        //    id = 0;
    }

    private void EngineerLogin_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Retrieve the engineer object from the database based on the entered id
            BO.Engineer? engineer = s_bl.Engineer.Read(id);

            // Check if an engineer with the entered id and name exists
            if (engineer != null && engineer.Name == EngineerName)
            {
                // If the engineer exists, close the login window and open the engineer view window
                Close();
                new EngineerView(id).Show(); 
            }
            else 
            {
                // If the engineer does not exist, display a message
                MessageBox.Show("User does not exist");
            }
        }
        catch
        {
            // Handle any exceptions
            MessageBox.Show("User does not exist");
        }
    }
}
