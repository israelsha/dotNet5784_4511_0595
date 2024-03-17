using BO; // Importing Business Objects namespace.
using PL.Engineer; // Importing Presentation Layer Engineer namespace.
using PL.Task; // Importing Presentation Layer Task namespace.
using System.Collections.ObjectModel; // Importing ObservableCollection class.
using System.Windows; // Importing Windows namespace.
using System.Windows.Controls; // Importing Windows Controls namespace.

namespace PL.Task
{
    /// <summary>
    /// Window for adding or updating tasks.
    /// </summary>
    public partial class TaskAddOrUpdate : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // Static reference to the business logic layer.

        // Dependency property for all tasks (for the dependent task).
        // This property holds a collection of TaskInList objects.
        public IEnumerable<TaskInList> TaskInList
        {
            get { return (IEnumerable<TaskInList>)GetValue(TaskInListProperty); }
            set { SetValue(TaskInListProperty, value); }
        }
        public static readonly DependencyProperty TaskInListProperty =
            DependencyProperty.Register("TaskInList", typeof(IEnumerable<TaskInList>), typeof(TaskAddOrUpdate), new PropertyMetadata(s_bl.Task.ReadAll()));

        // Dependency property for the current task.
        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }
        public static readonly DependencyProperty CurrentTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskAddOrUpdate), new PropertyMetadata(null));

        // Dependency property for EngineerId.
        public string? EngineerId
        {
            get { return (string)GetValue(EngineerIdProperty); }
            set { SetValue(EngineerIdProperty, value); }
        }
        public static readonly DependencyProperty EngineerIdProperty =
        DependencyProperty.Register("EngineerId", typeof(string), typeof(TaskAddOrUpdate), new PropertyMetadata(null));

        // ObservableCollection for all the dependent tasks.
        public ObservableCollection<int> SelectedIds
        {
            get { return (ObservableCollection<int>)GetValue(SelectedIdsProperty); }
            set { SetValue(SelectedIdsProperty, value); }
        }
        public static readonly DependencyProperty SelectedIdsProperty =
        DependencyProperty.Register("SelectedIds", typeof(ObservableCollection<int>), typeof(TaskAddOrUpdate), new PropertyMetadata(null));

        // Event handler for CheckBox checked state.
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Add the checked task ID to SelectedIds.
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.Content is int id)
            {
                if (!SelectedIds.Contains(id))
                    SelectedIds.Add(id);
            }
        }

        // Event handler for CheckBox unchecked state.
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Remove the unchecked task ID from SelectedIds.
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.Content is int id)
            {
                SelectedIds.Remove(id);
            }
        }

        // Event handler for CheckBox loaded state.
        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Set CheckBox checked state based on SelectedIds.
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.Content is int id && SelectedIds.Contains(id))
            {
                checkBox.IsChecked = true;
            }
        }

        public bool Flag = false;//if flag is false it is meens tha we came from task window and if true it is meens that we came from engineer window
        public TaskAddOrUpdate(int Id = 0, bool flag = false)
        {
            InitializeComponent();

            SelectedIds = new ObservableCollection<int>();

            if (Id == 0)
            {
                // Creates a new task with default values if Id is 0.
                CurrentTask = new BO.Task
                {
                    Id = 0,
                    Description = null,
                    Alias = null,
                    Dependencies = null,
                    RequiredEffortTime = null,
                    Deliverables = null,
                    Remarks = null,
                    Engineer = null,
                    Copmlexity = BO.EngineerExperience.None,
                };
            }
            else
            {
                // Retrieves the task with the given Id and initializes the list of dependencies if they exist.
                CurrentTask = s_bl.Task.Read(Id);

                if (CurrentTask!.Dependencies != null && CurrentTask.Dependencies.Count != 0)
                    SelectedIds = new ObservableCollection<int>(CurrentTask.Dependencies.Select(d => d.Id));
            }
            // Sets the EngineerId based on the CurrentTask, or null if the CurrentTask has no engineer assigned.
            EngineerId = (CurrentTask!.Engineer == null) ? null : CurrentTask.Engineer.Id.ToString();
            Flag = flag; // Sets the Flag value indicating the window origin.
        }


        private void AddOrUpdate_Button(object sender, RoutedEventArgs e)
        {
            try
            {
                // Convert the EngineerId from string to int.
                if (int.TryParse(EngineerId, out int engineerId))
                {
                    // Set the Engineer property of the CurrentTask.
                    CurrentTask.Engineer = new BO.EngineerInTask { Id = engineerId, Name = s_bl.Engineer.Read(engineerId).Name };
                }
                // Check if the engineer ID cannot be changed when the Flag is true.
                else if (Flag == true && CurrentTask.Engineer!.Id != engineerId)
                {
                    throw new Exception("You can't change the engineer ID");
                }
                // Set the Engineer property to null if the EngineerId is empty or null.
                else if (EngineerId == "" || EngineerId == null)
                {
                    CurrentTask.Engineer = null;
                }
                // Throw an exception for an invalid EngineerId.
                else
                {
                    throw new Exception("Invalid Engineer ID");
                }

                // Construct a list of dependencies for the current task.
                List<TaskInList> dependencies = new List<TaskInList>();
                foreach (var id in SelectedIds)
                {
                    // Retrieve each dependency task and add it to the list.
                    BO.Task taskOfDependency = s_bl.Task.Read(id);
                    dependencies.Add(new BO.TaskInList()
                    {
                        Id = taskOfDependency.Id,
                        Description = taskOfDependency.Description,
                        Alias = taskOfDependency.Alias,
                        Status = taskOfDependency.Status ?? 0
                    });
                }

                // Determine the button text.
                string? buttonText = (sender as Button)?.Content?.ToString();

                // Create a new task object based on user input.
                BO.Task task = new BO.Task
                {
                    Id = CurrentTask.Id,
                    Description = (CurrentTask.Description is string) ? CurrentTask.Description : throw new Exception("Must fill the Description box"),
                    Alias = (CurrentTask.Alias is string) ? CurrentTask.Alias : throw new Exception("Must fill the Alias box"),
                    CreatedAtDate = (buttonText == "Add") ? s_bl.Clock : CurrentTask.CreatedAtDate,
                    Status = BO.Status.Unscheduled,
                    Dependencies = dependencies,
                    RequiredEffortTime = CurrentTask.RequiredEffortTime,
                    StartDate = CurrentTask.StartDate ?? null,
                    ScheduledDate = CurrentTask.ScheduledDate ?? null,
                    ForecastDate = null,
                    DeadlineDate = CurrentTask.DeadlineDate ?? null,
                    Deliverables = CurrentTask.Deliverables ?? null,
                    Remarks = CurrentTask.Remarks,
                    Engineer = CurrentTask.Engineer,
                    Copmlexity = CurrentTask.Copmlexity
                };

                // Perform task addition or update based on the button text.
                if (buttonText == "Add")
                {
                    s_bl.Task.Create(task!);
                    MessageBox.Show("The task was successfully added");
                }
                else if (buttonText == "Update")
                {
                    s_bl.Task.Update(task!);
                    MessageBox.Show("The task was successfully updated");
                }

                // Close the window and open either the TaskForListWindow or EngineerView based on the Flag.
                Close();
                if (!Flag) // Flag is false, continue in the task window.
                    new TaskForListWindow().Show();
                else // Flag is true, continue in the engineer window.
                    new EngineerView(CurrentTask.Engineer.Id).Show();
            }
            catch (Exception ex)
            {
                // Display an error message box if an exception occurs during task addition or update.
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Home_Click(object sender, RoutedEventArgs e)//pass to the the home page 
        {
            if (!Flag) //bexk to admnin view
            {
                Close();
            }
            else  //beck to engineer view
            {
                Close();
                new EngineerView(CurrentTask.Engineer!.Id).Show();
            }
        }
    }
}
