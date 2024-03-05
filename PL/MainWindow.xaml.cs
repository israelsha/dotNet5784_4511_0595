using System.Text;
using System.Windows;
using System.Windows.Controls;
using PL.Engineer;
using PL.Task;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private void User_List_Click(object sender, RoutedEventArgs e)
        {
           new AdminLoginWindow().Show();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            new EngineerLoginWindow().Show();
        }
       



    }
}