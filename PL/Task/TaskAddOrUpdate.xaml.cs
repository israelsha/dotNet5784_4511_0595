using BO;
using PL.Engineer;
using PL.Task;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Input;

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


    public TaskAddOrUpdate(int Id = 0)
    {
        InitializeComponent();
        foreach (var task in s_bl.Task.ReadAll())
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Content = task.Id + " " + task.Alias;
            ComboBoxItem item = new ComboBoxItem();
            item.Content = checkBox;
            //dependencies.Items.Add(item);
        }
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
                Engineer = new EngineerInTask(),
                Copmlexity = BO.EngineerExperience.None,
            };
        }
        else
            CurrentTask = s_bl.Task.Read(Id);
    }

    private void AddOrUpdate_Button(object sender, RoutedEventArgs e)
    {
        try
        {
            string? buttonText = (sender as Button)?.Content?.ToString();


            List<TaskInList>? dependency = new List<TaskInList>();
            if (CurrentTask.Dependencies != null) ;

            BO.Task task = new BO.Task
            {
                Id = CurrentTask.Id,
                Description = (CurrentTask.Description is string) ? CurrentTask.Description : throw new Exception("Must fill the Description box"),
                Alias = (CurrentTask.Alias is string) ? CurrentTask.Alias : throw new Exception("Must fill the Alias box"),
                CreatedAtDate = (buttonText == "Add") ? DateTime.Now : CurrentTask.CreatedAtDate,
                Status = BO.Status.Unscheduled,
                Dependencies = (dependency.Count == 0) ? null : dependency,
                RequiredEffortTime = CurrentTask.RequiredEffortTime,
                StartDate = CurrentTask.StartDate ?? null,
                ScheduledDate = CurrentTask.ScheduledDate??null,
                ForecastDate = null,
                DeadlineDate = CurrentTask.DeadlineDate??null,
                Deliverables = CurrentTask.Deliverables ?? null,
                Remarks = CurrentTask.Remarks,
                Engineer = new BO.EngineerInTask { Id = CurrentTask.Engineer.Id, Name = s_bl.Engineer.Read(CurrentTask.Engineer.Id).Name },
                Copmlexity=CurrentTask.Copmlexity
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
            new TaskForListWindow().Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }



    
   
}
