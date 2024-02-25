using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
            TaskList = (Level == BO.EngineerExperience.None) ?
            s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => (int)item.Copmlexity == (int)Level)!;
        }

        private void AddTask_Button(object sender, RoutedEventArgs e)
        {
                Close();
                new TaskWindow().ShowDialog();
        }
        private void UpdateTask_Button(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BO.Task? tsk = (sender as ListView)?.SelectedItem as BO.Task;
            if (tsk != null)
            {
                Close();
                new TaskWindow().ShowDialog();
            }
            Close();
            new TaskWindow().ShowDialog();
        }

        public BO.Status Status { get; set; } = BO.Status.None;

        private void status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Status == BO.Status.None) ?
            s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => (int)item.Copmlexity == (int)Status)!;
        }
    }
}
