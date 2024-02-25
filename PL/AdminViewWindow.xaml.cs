using PL.Engineer;
using PL.Task;
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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for AdminViewWindow.xaml
    /// </summary>
    public partial class AdminViewWindow : Window
    {
        public AdminViewWindow()
        {
            InitializeComponent();
        }

        private void Task_Click(object sender, RoutedEventArgs e)
        {
            new TaskForListWindow().Show();
        }
        private void Engineer_Click(object sender, RoutedEventArgs e)
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string message = "Are you sure you want to reset the data?";
            string title = "Reset Data";
            MessageBoxButton buttons = MessageBoxButton.YesNo;

            MessageBoxResult result = MessageBox.Show(message, title, buttons);
            if (result == MessageBoxResult.Yes)
            {
                DalTest.Initialization.initialize();
            }
        }
    }
}
