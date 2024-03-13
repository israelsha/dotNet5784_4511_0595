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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for TaskForEngineer.xaml
    /// </summary>
    public partial class TaskForEngineer : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }
        public static readonly DependencyProperty TaskListProperty =
       DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskForEngineer), new PropertyMetadata(null));

        public BO.Engineer currentEngineer=new BO.Engineer();
        public TaskForEngineer(BO.Engineer CurrentEngineer)
        {
            InitializeComponent();
            TaskList = s_bl.Task.ReadAll(item => (int)item.Copmlexity <= (int)CurrentEngineer.Level && item.EngineerId == null && item.StartDate == null);
            currentEngineer = CurrentEngineer;
        }

        private void ChooseTask_Button(object sender, MouseButtonEventArgs e)
        {
            if(s_bl.Dates.getStartProject()==null)
            {
                MessageBox.Show("A schedule of tasks needs to be set up first");
                return;
            }
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            Close();
            new AddTaskForEngineer(currentEngineer,task.Id).ShowDialog();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new EngineerView(currentEngineer.Id).ShowDialog();
        }
    }
}
