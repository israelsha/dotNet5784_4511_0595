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
            TaskList= s_bl.Task.ReadAll();
        }

        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }
        public static readonly DependencyProperty TaskListProperty =
       DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskForListWindow), new PropertyMetadata(null));

        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;

        private void Level_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
                           where s_bl.Task.Read(item.Id).Copmlexity == Level && (item.Status == Status||Status==BO.Status.None)
                           select item;
            }
        }

        private void AddTask_Button(object sender, RoutedEventArgs e)
        {
                Close();
                new TaskAddOrUpdate(0).ShowDialog(); 
        }

        private void UpdateTask_Button(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (task != null)
            {
                Close();
                new TaskAddOrUpdate(task.Id).ShowDialog();
            }
           
        }

        public BO.Status Status { get; set; } = BO.Status.None;

        private void status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Status == BO.Status.None)
            {
                TaskList = (Level == BO.EngineerExperience.None) ?
                 s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => (int)item.Copmlexity == (int)Level)!;
            }
            else
            {
                IEnumerable<BO.TaskInList> task = s_bl?.Task.ReadAll();
                TaskList = from item in task
                           where (s_bl.Task.Read(item.Id).Copmlexity == Level||Level==BO.EngineerExperience.None) && item.Status == Status
                           select item;
            }
            
        }

      
    }
}
