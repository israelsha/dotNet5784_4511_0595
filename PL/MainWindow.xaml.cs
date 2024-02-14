using System.Text;
using System.Windows;
using PL.Engineer;
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
        private void Engineer_List_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }
        private void Initialization_Click(object sender, RoutedEventArgs e)
        {
            string message = "Are you sure you want to initialize the data?";
            string title = "Initialize Data";
            MessageBoxButton buttons = MessageBoxButton.YesNo;

            MessageBoxResult result = MessageBox.Show(message, title, buttons);
            if (result == MessageBoxResult.Yes)
            {
                DalTest.Initialization.initialize();
                DalTest.Initialization.Do();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}