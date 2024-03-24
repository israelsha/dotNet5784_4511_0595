using PL.Engineer;  // Importing namespace for Engineer related functionalities.
using PL.Task;      // Importing namespace for Task related functionalities.
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
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();  // Initializing static reference to the Business Logic layer.

        public AdminViewWindow()
        {
            InitializeComponent();  // Initializing components for the Admin View window.
        }

        // Handler for Task button click event.
        private void Task_Click(object sender, RoutedEventArgs e)
        {
            new TaskForListWindow().Show();  // Showing Task List window.
        }

        // Handler for Engineer button click event.
        private void Engineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();  // Showing Engineer List window.
        }

        // Handler for Initialization button click event.
        private void Initialization_Click(object sender, RoutedEventArgs e)
        {
            // Confirmation message for initializing data.
            string message = "Are you sure you want to initialize the data?";
            string title = "Initialize Data";
            MessageBoxButton buttons = MessageBoxButton.YesNo;

            // Showing confirmation dialog and performing initialization if confirmed.
            MessageBoxResult result = MessageBox.Show(message, title, buttons);
            if (result == MessageBoxResult.Yes)
            {
                DalTest.Initialization.initialize();  // Initializing data.
                DalTest.Initialization.Do();           // Performing initialization action.
            }
        }

        // Handler for Delete button click event.
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Confirmation message for resetting data.
            string message = "Are you sure you want to reset the data?";
            string title = "Reset Data";
            MessageBoxButton buttons = MessageBoxButton.YesNo;

            // Showing confirmation dialog and resetting data if confirmed.
            MessageBoxResult result = MessageBox.Show(message, title, buttons);
            if (result == MessageBoxResult.Yes)
            {
                DalTest.Initialization.initialize();  // Resetting data.
            }
        }

        // Handler for opening Date Picker Popup.
        private void OpenDatePickerPopup(object sender, RoutedEventArgs e)
        {
            if(s_bl.Dates.getStartProject().HasValue)
            {
                MessageBox.Show("A schedule has already been set ");  // Notifying if a schedule is already set.
                return;
            }
            datePickerPopup.IsOpen = true;  // Opening Date Picker Popup.
        }

        // Handler for setting selected date.
        private void SetDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime? selectedDate = scheduleDatePicker.SelectedDate;  // Getting selected date from Date Picker.

                if (selectedDate.HasValue)
                {
                    datePickerPopup.IsOpen = false;  // Closing Date Picker Popup after selecting a date.
                    
                    // Resetting and setting start project date in the Business Logic layer.
                    s_bl.Dates.resetDate(selectedDate ?? throw new Exception("Not valid date"));
                    s_bl.Dates.setStartProject(selectedDate ?? throw new Exception("Not valid date"));

                    MessageBox.Show($"Selected Date: {selectedDate.Value.ToShortDateString()}");  // Showing selected date.
                }
                else
                {
                    MessageBox.Show("Please select a date.");  // Notifying if no date is selected.
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);  // Handling and displaying any exceptions occurred.
            }
        }

        // Handler for Cancel button click event.
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            datePickerPopup.IsOpen = false;  // Closing Date Picker Popup.
        }

        // Handler for opening Ganttchart.
        private void OpenGanttchart(object sender, RoutedEventArgs e)
        {
            if (s_bl.Dates.getStartProject().HasValue==false)
            {
                MessageBox.Show("A schedule has to be set first")   ;  // Notifying if a schedule is already set.
                return;
            }
            new GanttchartWindow().Show();  // Opening Ganttchart window.
        }
    }
}
