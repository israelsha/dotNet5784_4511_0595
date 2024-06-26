﻿using BO;
using System.Windows;
using PL.Task;
using System.Windows.Navigation;
using System.ComponentModel;
namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerView.xaml
/// </summary>
public partial class EngineerView : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // Initializing s_bl with Factory.Get()



    // Dependency Property for TaskInList
    public IEnumerable<TaskInList> TaskInList
    {
        get { return (IEnumerable<TaskInList>)GetValue(TaskInListProperty); }
        set { SetValue(TaskInListProperty, value); }
    }
    public static readonly DependencyProperty TaskInListProperty =
        DependencyProperty.Register("TaskInList", typeof(IEnumerable<TaskInList>), typeof(EngineerView), new PropertyMetadata(s_bl.Task.ReadAll()));

    // Dependency Property for CurrentEngineer
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

    // Constructor for EngineerView
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
            CurrentTask = new BO.Task{ Id = 0 };  // Creating a new Task if an exception occurs
        }

    }

    private void UpdateTask_Button(object sender, RoutedEventArgs e)
    {
        if (CurrentTask.Id == 0)
        {
            MessageBox.Show("You don't have a task yet");
            return;
        }
        if(s_bl.Dates.getStartProject()==null)
        {
            MessageBox.Show("A schedule of tasks needs to be set up first");
            return;
        }

        try
        {
            bool flag = true;
            Close();
            new TaskAddOrUpdate(CurrentTask.Id,flag).ShowDialog();
        }
        catch(Exception ex) 
        {
            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    private void TaskOption_Button(object sender, RoutedEventArgs e)
    {
        if (CurrentTask.Id != 0)    //check if the current task was finished
            MessageBox.Show("you need to finish your task first");
        else      //shoes the task that the engineer can do
        {
            Close();
            new TaskForEngineer(CurrentEngineer).Show();
        }

    }

    private void TaskCompleted_Button(object sender, RoutedEventArgs e)
    {
        if(CurrentTask.Id == 0)
        {
            MessageBox.Show("You don't have a task yet"); ; ; ; ;
            return;
        }
        try
        {
            CurrentTask.CompleteDate = s_bl.Clock;// Setting CompleteDate to current date
            CurrentTask.Engineer = null;    // Clearing Engineer
            s_bl.Task.Update(CurrentTask);  // Updating Task
            MessageBox.Show("Well done, you have successfully completed the task");
            Close();
            new EngineerView(CurrentEngineer.Id).Show();
        }
        catch { MessageBox.Show("Error"); }// Show error message if an exception occurs

    }
}
