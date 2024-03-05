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
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

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

        private void OpenDatePickerPopup(object sender, RoutedEventArgs e)
        {
            datePickerPopup.IsOpen = true;
        }

        private void SetDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                DateTime? selectedDate = scheduleDatePicker.SelectedDate;

                if (selectedDate.HasValue)
                {
                    datePickerPopup.IsOpen = false;  //close selected date 
                    MessageBox.Show($"Selected Date: {selectedDate.Value.ToShortDateString()}");

                    s_bl.Task.resetDate(selectedDate ?? throw new Exception("Not valid date"));
                }
                else
                {
                    MessageBox.Show("Please select a date.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            datePickerPopup.IsOpen = false;
        }

        private void OpenGanttchart(object sender, RoutedEventArgs e)
        {
            new GanttchartWindow().Show();
        }
    }
}
