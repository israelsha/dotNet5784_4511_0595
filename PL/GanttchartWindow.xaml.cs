using PL.Task;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for GanttchartWindow.xaml
    /// </summary>
    public partial class GanttchartWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public GanttchartWindow()
        {
            InitializeComponent();
        }
          public IEnumerable<BO.TaskInList> TaskList      //list for all the task
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }
        public static readonly DependencyProperty TaskListProperty =
       DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskForListWindow), new PropertyMetadata(s_bl.Task.ReadAll()));

        class DateToWidthConverter : IValueConverter
        {
            static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); //get the Bl instance

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is BO.Task task)
                {
                    TimeSpan requiredEffortTime = (TimeSpan)task.RequiredEffortTime!; //get the task duration
                    TimeSpan allProjectDuration;
                    try
                    {
                        allProjectDuration = (TimeSpan)((DateTime)s_bl.Dates.getEndProject()! - (DateTime)s_bl.Dates.getStartProject()!)!; //get the project duration
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        return 0;

                    }

                    return (requiredEffortTime / allProjectDuration) * 1000; //return the task width
                }
                return 0;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }


        class DateToMarginConverter : IValueConverter
        {
            static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); //get the Bl instance

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BO.Task task)
            {
                DateTime taskStartDate = (DateTime)task.ScheduledDate!; //get the task duration

                TimeSpan allProjectDuration;
                DateTime startDate;
                DateTime endDate;

                try
                {
                    endDate = (DateTime)s_bl.Dates.getEndProject()!;
                    startDate = (DateTime)s_bl.Dates.getStartProject()!;
                    allProjectDuration = (TimeSpan)(endDate - startDate); //get the project duration
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return 0;

                }

                return new Thickness((((TimeSpan)(taskStartDate - startDate) / allProjectDuration) * 1000), 0, 0, 0); //return the task margin
            }
            return 0;
        }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }


        class StatusToColorConverter : IValueConverter
        {
            static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); //get the Bl instance

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is BO.Task task)
                {
                    if (task.Dependencies != null)
                    {
                        foreach (var item in task.Dependencies)
                        {
                            if (task.Status == BO.Status.InJeopardy)
                                return "#e16d70";
                        }
                    }
                    if (task.Status == BO.Status.InJeopardy)
                        return "#e16d70";
                    if (task.Status == BO.Status.Done)
                        return "#bcc771";
                    if (task.Status == BO.Status.OnTrack)
                        return "#60b0d1";
                    if (task.Status == BO.Status.Scheduled)
                        return "#ecbe62";
                    if (task.Status == BO.Status.Unscheduled)
                        return "#cfbc79";

                }
                return 0;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }


    }
}
