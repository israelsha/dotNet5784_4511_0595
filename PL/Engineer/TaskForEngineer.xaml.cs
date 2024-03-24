using PL.Task;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for TaskForEngineer.xaml
    /// </summary>
    public partial class TaskForEngineer : Window
    {
        // Access the business logic layer
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // Dependency property for binding the list of tasks
        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskForEngineer), new PropertyMetadata(null));

        // Current engineer for whom the tasks are being displayed
        public BO.Engineer currentEngineer = new BO.Engineer();

        // Constructor for TaskForEngineer window
        public TaskForEngineer(BO.Engineer CurrentEngineer)
        {
            InitializeComponent();
            // Retrieve tasks based on engineer's level and availability
            IEnumerable < BO.TaskInList > tasks = s_bl.Task.ReadAll(item => (int)item.Copmlexity <= (int)CurrentEngineer.Level && item.EngineerId == null && item.StartDate == null);

            //Selects all tasks that do not depend on tasks that have not finished yet
            TaskList = tasks.Where(task =>
            {
                var dependencies = s_bl.Task.Read(task.Id).Dependencies;
                if (dependencies == null || dependencies.Count() == 0)
                    return true; // If there are no dependencies, the task is appropriate
                                 // If there are dependencies, check that all dependent tasks have been completed
                return dependencies.All(dep => s_bl.Task.Read(dep.Id).CompleteDate.HasValue);
            }).ToList();
            currentEngineer = CurrentEngineer;
        }

        // Event handler for selecting a task from the list
        private void ChooseTask_Button(object sender, MouseButtonEventArgs e)
        {
            // Check if a schedule of tasks has been set up
            if (s_bl.Dates.getStartProject() == null)
            {
                MessageBox.Show("A schedule of tasks needs to be set up first");
                return;
            }
            // Get the selected task
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            // Close the current window and open the window to add task for engineer
            Close();
            new AddTaskForEngineer(currentEngineer, task.Id).ShowDialog();
        }

        // Event handler for the Home button click
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            // Close the current window and open the EngineerView window for the current engineer
            Close();
            new EngineerView(currentEngineer.Id).ShowDialog();
        }
    }
}
