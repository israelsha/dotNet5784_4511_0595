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
       
          public IEnumerable<BO.Task> TaskListGant      //list for all the task
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListGantProperty); }
            set { SetValue(TaskListGantProperty, value); }
        }
        public static readonly DependencyProperty TaskListGantProperty =
       DependencyProperty.Register("TaskListGant", typeof(IEnumerable<BO.Task>), typeof(GanttchartWindow), new PropertyMetadata(null));


        public GanttchartWindow()
        {
            InitializeComponent();
            IEnumerable<BO.TaskInList> tasks = s_bl.Task.ReadAll();
            TaskListGant = tasks.Select(task => s_bl.Task.Read(task.Id)).ToList();




            // תאריכי התחלה וסיום של הפרוייקט
            DateTime ?startDate = s_bl.Dates.getStartProject(); // תאריך התחלה
            DateTime? endDate = s_bl.Dates.getEndProject(); // תאריך סיום

            // קביעת ערכי גובה ורוחב לפס הלוח שנה
            double calendarWidth = 1000; // רוחב כללי של הלוח שנה
            double cellWidth = calendarWidth / ((endDate - startDate)!.Value.Days + 1);
            GridLength cellLength = new GridLength(calendarWidth / ((endDate - startDate)!.Value.Days + 1), GridUnitType.Pixel);


            // יצירת פס הלוח שנה
            Canvas calendarCanvas = new Canvas();
            calendarCanvas.Height = 50;
            calendarCanvas.Background = Brushes.LightGray;
            calendarCanvas.VerticalAlignment = VerticalAlignment.Center;

            // הוספת קווים של השנה
            Line yearLine = new Line();
            yearLine.X1 = 0;
            yearLine.Y1 = 25;
            yearLine.X2 = calendarWidth;
            yearLine.Y2 = 25;
            yearLine.Stroke = Brushes.Black;
            yearLine.StrokeThickness = 1;
            calendarCanvas.Children.Add(yearLine);

            // הוספת סימוני חודשים
            // כאן תוכל להוסיף את הקווים או הטקסט של החודשים בצורה שמתאימה לך

            // הוספת הלוח שנה לרכיב הגריד
            Grid.SetRow(calendarCanvas, 1); 
            // הוספת הלוח השנה ל-Grid שכבר קיים בתוך ה-XAML
            Grid parentGrid = (Grid)this.Content; // קבלת ה-Grid המצוי בתוך ה-XAML
            parentGrid.Children.Add(calendarCanvas); // הוספת הלוח השנה לתוך ה-Grid

            // כאן תוכל להוסיף את שאר הרכיבים לפי הצורך

            for (DateTime? date = startDate; date <= endDate; date = date.Value.AddMonths(1))
            {
                int month = date.Value.Month;
                double xPos = (date - startDate)!.Value.Days * cellWidth;

                Line monthLine = new Line();
                monthLine.X1 = xPos;
                monthLine.Y1 = 0;
                monthLine.X2 = xPos;
                monthLine.Y2 = 50;
                monthLine.Stroke = Brushes.Black;
                monthLine.StrokeThickness = 1;
                calendarCanvas.Children.Add(monthLine);

                TextBlock monthText = new TextBlock();
                monthText.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);
                monthText.Margin = new Thickness(xPos, 0, 0, 0);
                calendarCanvas.Children.Add(monthText);
            }

        }




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
