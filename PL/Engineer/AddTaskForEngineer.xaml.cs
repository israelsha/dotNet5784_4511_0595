using BO;
using System.Windows;


namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for AddTaskForEngineer.xaml
    /// </summary>
    public partial class AddTaskForEngineer : Window
    {
        // Accessing the Business Logic layer
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // Dependency property for the current task
        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }
        public static readonly DependencyProperty CurrentTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(AddTaskForEngineer), new PropertyMetadata(null));

        // Dependency property for the list of tasks
        public IEnumerable<TaskInList> TaskInList
        {
            get { return (IEnumerable<TaskInList>)GetValue(TaskInListProperty); }
            set { SetValue(TaskInListProperty, value); }
        }
        public static readonly DependencyProperty TaskInListProperty =
            DependencyProperty.Register("TaskInList", typeof(IEnumerable<TaskInList>), typeof(AddTaskForEngineer), new PropertyMetadata(s_bl.Task.ReadAll()));

        // Current engineer instance
        public BO.Engineer currentEngineer = new BO.Engineer();

        // Constructor
        public AddTaskForEngineer(BO.Engineer CurrentEngineer, int id)
        {
            try
            {
                InitializeComponent();
                CurrentTask = s_bl.Task.Read(id);
                currentEngineer = CurrentEngineer;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for adding a task to an engineer
        private void addTaskToEngineer_Button(object sender, RoutedEventArgs e)
        {
            CurrentTask.StartDate = s_bl.Clock;
            CurrentTask.Status = BO.Status.OnTrack;
            CurrentTask.Engineer = new EngineerInTask { Id = currentEngineer.Id, Name = currentEngineer.Name };
            s_bl.Task.Update(CurrentTask);
            Close();
            MessageBox.Show("The task was successfully added to you");
            new EngineerView(currentEngineer.Id).Show();
        }

        // Event handler for navigating to the home screen
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new EngineerView(currentEngineer.Id).ShowDialog();
        }
    }
}
