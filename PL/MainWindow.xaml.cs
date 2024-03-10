using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using PL.Engineer;
using PL.Task;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        static int countHour = 0;
        static int countmonth = 0;
        private string _currentDateTime;
        public string CurrentDateTime
        {
            get { return _currentDateTime; }
            set
            {
                _currentDateTime = value;
                OnPropertyChanged("CurrentDateTime");
            }
        }

        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            UpdateDateTime();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!_updatingDateTime)
                UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            DateTime dt = DateTime.Now;
            dt = dt.AddHours(countHour);
            dt = dt.AddMonths(countmonth);
            CurrentDateTime = dt.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _updatingDateTime = false;

        private void Move_forward_one_hour_Click(object sender, RoutedEventArgs e)
        {
            countHour++;
        }
        private void Move_beckward_one_hour_Click(object sender, RoutedEventArgs e)
        {
            countHour--;
        }

        private void Move_forward_one_day_Click(object sender, RoutedEventArgs e)
        {
            countHour += 24;
        }
        private void Move_beckward_one_day_Click(object sender, RoutedEventArgs e)
        {
            countHour -= 24;
        }

        private void Move_forward_one_month_Click(object sender, RoutedEventArgs e)
        {
            countmonth++;
        }
        private void Move_beckward_one_month_Click(object sender, RoutedEventArgs e)
        {
            countmonth--;
        }
        private void UpdateTimer()
        {
            _timer.Stop();
            _timer.Start();
        }

        //move to maneger login
        private void User_List_Click(object sender, RoutedEventArgs e)
        {
           new AdminLoginWindow().Show();
        }

        //move to engineer login
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            new EngineerLoginWindow().Show();
        }       
    }
}