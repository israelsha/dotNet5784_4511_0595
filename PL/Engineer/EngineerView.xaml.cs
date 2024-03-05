using BO;
using System.Windows;

namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerView.xaml
/// </summary>
public partial class EngineerView : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();



    public IEnumerable<TaskInList> TaskInList
    {
        get { return (IEnumerable<TaskInList>)GetValue(TaskInListProperty); }
        set { SetValue(TaskInListProperty, value); }
    }
    public static readonly DependencyProperty TaskInListProperty =
        DependencyProperty.Register("TaskInList", typeof(IEnumerable<TaskInList>), typeof(EngineerView), new PropertyMetadata(s_bl.Task.ReadAll()));

    public BO.Engineer CurrentEngineer
    {
        get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
        set { SetValue(CurrentEngineerProperty, value); }
    }
    public static readonly DependencyProperty CurrentEngineerProperty =
        DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerView), new PropertyMetadata(null));


    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(CurrentTaskProperty); }
        set { SetValue(CurrentTaskProperty, value); }
    }
    public static readonly DependencyProperty CurrentTaskProperty =
    DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(EngineerView), new PropertyMetadata(null));


    public EngineerView(int Id)
    {
        InitializeComponent();
        try
        {
            CurrentEngineer = s_bl.Engineer.Read(Id);
            CurrentTask = s_bl.Task.Read(0, item => item.EngineerId == Id);
        }
        catch
        {
            CurrentTask = new BO.Task{ Id = 0 };

        }

    }

    private void UpdateTask_Button(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Task.Update(CurrentTask);
            MessageBox.Show("The task was successfully updated");
        }
        catch
        {
            MessageBox.Show("Error");
            Close();
        }

    }

    private void TaskOption_Button(object sender, RoutedEventArgs e)
    {

    }
}
