using BO;
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

namespace PL.Engineer;

/// <summary>
/// Interaction logic for AddTaskForEngineer.xaml
/// </summary>
public partial class AddTaskForEngineer : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(CurrentTaskProperty); }
        set { SetValue(CurrentTaskProperty, value); }
    }
    public static readonly DependencyProperty CurrentTaskProperty =
    DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(AddTaskForEngineer), new PropertyMetadata(null));

    public IEnumerable<TaskInList> TaskInList
    {
        get { return (IEnumerable<TaskInList>)GetValue(TaskInListProperty); }
        set { SetValue(TaskInListProperty, value); }
    }
    public static readonly DependencyProperty TaskInListProperty =
        DependencyProperty.Register("TaskInList", typeof(IEnumerable<TaskInList>), typeof(AddTaskForEngineer), new PropertyMetadata(s_bl.Task.ReadAll()));

    public BO.Engineer currentEngineer = new BO.Engineer();
    public AddTaskForEngineer(BO.Engineer CurrentEngineer,int id)
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

    private void addTaskToEngineer_Button(object sender, RoutedEventArgs e)
    {
        CurrentTask.StartDate = DateTime.Now;
        CurrentTask.Engineer = new EngineerInTask { Id = currentEngineer.Id,Name=currentEngineer.Name };
        s_bl.Task.Update(CurrentTask);
        Close();
        MessageBox.Show("The task was successfully added to you");
        new EngineerView(currentEngineer.Id).Show();
    }
}
