using PL.Task; 
using System.Globalization; 
using System.Windows; 
using System.Windows.Controls; 
using System.Windows.Data; 

namespace PL
{
    /// <summary>
    /// Interaction logic for GanttchartWindow.xaml
    /// </summary>
    public partial class GanttchartWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // Static Bl instance

        // List for all the tasks
        public IEnumerable<BO.Task> TaskListGant
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListGantProperty); }
            set { SetValue(TaskListGantProperty, value); }
        }

        // Dependency property for TaskListGant
        public static readonly DependencyProperty TaskListGantProperty =
       DependencyProperty.Register("TaskListGant", typeof(IEnumerable<BO.Task>), typeof(GanttchartWindow), new PropertyMetadata(null));


        public GanttchartWindow()
        {
            InitializeComponent(); // Initializing the component
            IEnumerable<BO.TaskInList> tasks = s_bl.Task.ReadAll(); // Reading all tasks
            TaskListGant = tasks.Select(task => s_bl.Task.Read(task.Id)).ToList(); // Converting tasks to list and setting TaskListGant
        }

        // Converter to calculate width based on task duration
        class DateToWidthConverter : IValueConverter
        {
            static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // Getting the Bl instance

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is BO.Task task)
                {
                    TimeSpan requiredEffortTime = (TimeSpan)task.RequiredEffortTime!; // Getting task duration
                    TimeSpan allProjectDuration;
                    try
                    {
                        allProjectDuration = (TimeSpan)((DateTime)s_bl.Dates.getEndProject()! - (DateTime)s_bl.Dates.getStartProject()!)!; // Getting project duration
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message); // Showing error message
                        return 0;

                    }

                    return (requiredEffortTime / allProjectDuration) * 1000; // Returning task width
                }
                return 0;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }


        // Converter to calculate margin based on task start date
        class DateToMarginConverter : IValueConverter
        {
            static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // Getting the Bl instance

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is BO.Task task)
                {
                    DateTime taskStartDate = (DateTime)task.ScheduledDate!; // Getting task start date

                    TimeSpan allProjectDuration;
                    DateTime startDate;
                    DateTime endDate;

                    try
                    {
                        endDate = (DateTime)s_bl.Dates.getEndProject()!;
                        startDate = (DateTime)s_bl.Dates.getStartProject()!;
                        allProjectDuration = (TimeSpan)(endDate - startDate); // Getting project duration
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message); // Showing error message
                        return 0;

                    }

                    return new Thickness((((TimeSpan)(taskStartDate - startDate) / allProjectDuration) * 1000), 0, 0, 0); // Returning task margin
                }
                return 0;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }


        // Converter to assign color based on task status
        class StatusToColorConverter : IValueConverter
        {
            static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // Getting the Bl instance

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is BO.Task task)
                {
                    if (task.Dependencies != null)
                    {
                        foreach (var item in task.Dependencies)
                        {
                            if (task.Status == BO.Status.InJeopardy)
                                return "#e16d70"; // Return color for InJeopardy status
                        }
                    }
                    if (task.Status == BO.Status.InJeopardy)
                        return "#e16d70"; // Return color for InJeopardy status
                    if (task.Status == BO.Status.Done)
                        return "#bcc771"; // Return color for Done status
                    if (task.Status == BO.Status.OnTrack)
                        return "#60b0d1"; // Return color for OnTrack status
                    if (task.Status == BO.Status.Scheduled)
                        return "#ecbe62"; // Return color for Scheduled status
                    if (task.Status == BO.Status.Unscheduled)
                        return "#cfbc79"; // Return color for Unscheduled status
                }
                return 0;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        // Event handler for Home button click
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Closing the window
            new AdminViewWindow().Show();
        }
    }
}
