using System.Windows;
using System.Windows.Controls;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskForListWindow.xaml
    /// </summary>
    public partial class TaskForListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public TaskForListWindow()
        {
            InitializeComponent();
            // Initialize the task list and project start date
            TaskList = s_bl.Task.ReadAll();
            startProject = s_bl.Dates.getStartProject();
        }

        // Dependency property for the task list
        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }
        public static readonly DependencyProperty TaskListProperty =
       DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskForListWindow), new PropertyMetadata(null));

        // Dependency property for the project start date
        public DateTime? startProject
        {
            get { return (DateTime?)GetValue(startProjectProperty); }
            set { SetValue(startProjectProperty, value); }
        }
        public static readonly DependencyProperty startProjectProperty =
       DependencyProperty.Register("startProject", typeof(DateTime?), typeof(TaskForListWindow), new PropertyMetadata(s_bl.Dates.getStartProject()));

        // Property for the engineer experience level
        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;

        // Event handler for engineer level selection change
        private void Level_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update the task list based on selected engineer level and task status
            if (Level == BO.EngineerExperience.None)
            {
                TaskList = (Status == BO.Status.None) ?
                 s_bl?.Task.ReadAll()! : from item in s_bl.Task.ReadAll()
                                         where item.Status == Status
                                         select item;
            }
            else
            {
                TaskList = from item in s_bl?.Task.ReadAll()
                           where s_bl.Task.Read(item.Id).Copmlexity == Level && (item.Status == Status || Status == BO.Status.None)
                           select item;
            }
        }

        // Event handler for adding a new task
        private void AddTask_Button(object sender, RoutedEventArgs e)
        {
            Close();
            new TaskAddOrUpdate(0).ShowDialog();
        }

        // Event handler for updating a selected task
        private void UpdateTask_Button(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Open the task add/update window with the selected task ID for updating
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (task != null)
            {
                Close();
                new TaskAddOrUpdate(task.Id).ShowDialog();
            }

        }

        // Property for the task status
        public BO.Status Status { get; set; } = BO.Status.None;

        // Event handler for task status selection change
        private void status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update the task list based on selected task status and engineer level
            if (Status == BO.Status.None)
            {
                TaskList = (Level == BO.EngineerExperience.None) ?
                 s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => (int)item.Copmlexity == (int)Level)!;
            }
            else
            {
                IEnumerable<BO.TaskInList> task = s_bl?.Task.ReadAll();
                TaskList = from item in task
                           where (s_bl.Task.Read(item.Id).Copmlexity == Level || Level == BO.EngineerExperience.None) && item.Status == Status
                           select item;
            }

        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new AdminViewWindow().Show();
        }
    }
}
