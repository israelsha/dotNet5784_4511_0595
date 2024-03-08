using BO;
using PL.Engineer;
using PL.Task;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace PL.Task;

/// <summary>
/// Interaction logic for TaskAddOrUpdate.xaml
/// </summary>
public partial class TaskAddOrUpdate : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


    //DependencyProperty of all the task (for the dependent task)
    public IEnumerable<TaskInList> TaskInList
    {
        get { return (IEnumerable<TaskInList>)GetValue(TaskInListProperty); }
        set { SetValue(TaskInListProperty, value); }
    }
    public static readonly DependencyProperty TaskInListProperty =
        DependencyProperty.Register("TaskInList", typeof(IEnumerable<TaskInList>), typeof(TaskAddOrUpdate), new PropertyMetadata(s_bl.Task.ReadAll()));

    //DependencyProperty of this task 
    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(CurrentTaskProperty); }
        set { SetValue(CurrentTaskProperty, value); }
    }
    public static readonly DependencyProperty CurrentTaskProperty =
    DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskAddOrUpdate), new PropertyMetadata(null));

    public string? EngineerId
    {
        get { return (string)GetValue(EngineerIdProperty); }
        set { SetValue(EngineerIdProperty, value); }
    }
    public static readonly DependencyProperty EngineerIdProperty =
    DependencyProperty.Register("EngineerId", typeof(string), typeof(TaskAddOrUpdate), new PropertyMetadata(null));

    public ObservableCollection<int> SelectedIds  //ObservableCollection for all the dependent task
    {
        get { return (ObservableCollection<int>)GetValue(SelectedIdsProperty); }
        set { SetValue(SelectedIdsProperty, value); }
    }
    public static readonly DependencyProperty SelectedIdsProperty =
    DependencyProperty.Register("SelectedIds", typeof(ObservableCollection<int>), typeof(TaskAddOrUpdate), new PropertyMetadata(null));

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = sender as CheckBox;
        if (checkBox != null && checkBox.Content is int id)
        {
            if (!SelectedIds.Contains(id))
                SelectedIds.Add(id);
        }
    }
    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = sender as CheckBox;
        if (checkBox != null && checkBox.Content is int id)
        {
            SelectedIds.Remove(id);
        }
    }

    public bool Flag = false;
    public TaskAddOrUpdate(int Id = 0, bool flag = false)
    {
        InitializeComponent();

        SelectedIds = new ObservableCollection<int>();
      
        if (Id == 0)
        {
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
            CurrentTask = s_bl.Task.Read(Id);//initialize the current task

            if (CurrentTask!.Dependencies != null && CurrentTask.Dependencies.Count != 0) //initialize the list of the dependesy
                SelectedIds = new ObservableCollection<int>(CurrentTask.Dependencies.Select(d => d.Id));
        }
        EngineerId = (CurrentTask!.Engineer == null) ? null : CurrentTask.Engineer.Id.ToString();
        Flag = flag; 
    }


    private void AddOrUpdate_Button(object sender, RoutedEventArgs e)
    {
        try
        {
            //taking tha text from the EngineerId box and convert it from string to int
            if (int.TryParse(EngineerId, out int engineerId))
            {
                CurrentTask.Engineer = new BO.EngineerInTask { Id = engineerId, Name = s_bl.Engineer.Read(engineerId).Name };
            }
            else if (EngineerId == "" || EngineerId == null) CurrentTask.Engineer = null;
            else
            {
                MessageBox.Show("Invalid Engineer Id");
                return;
            }

            List<TaskInList> dependencies = new List<TaskInList>();
            foreach (var id in SelectedIds)
            {
                BO.Task taskOfDependency = s_bl.Task.Read(id);
                dependencies.Add(new BO.TaskInList()
                {
                    Id = taskOfDependency.Id,
                    Description = taskOfDependency.Description,
                    Alias = taskOfDependency.Alias,
                    Status = taskOfDependency.Status??0
                });
            }
            string? buttonText = (sender as Button)?.Content?.ToString();
            BO.Task task = new BO.Task
            {
                Id = CurrentTask.Id,
                Description = (CurrentTask.Description is string) ? CurrentTask.Description : throw new Exception("Must fill the Description box"),
                Alias = (CurrentTask.Alias is string) ? CurrentTask.Alias : throw new Exception("Must fill the Alias box"),
                CreatedAtDate = (buttonText == "Add") ? DateTime.Now : CurrentTask.CreatedAtDate,
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
                Copmlexity =CurrentTask.Copmlexity
            };
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
            Close();
            if (!Flag) new TaskForListWindow().Show();
            else new EngineerView(CurrentTask.Engineer.Id).Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
