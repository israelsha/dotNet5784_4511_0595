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

        static int count = 0;
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
            dt = dt.AddHours(count);
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
            count++;
        }

        private void Move_forward_one_day_Click(object sender, RoutedEventArgs e)
        {
            count += 24;
        }

        private void UpdateTimer()
        {
            _timer.Stop();
            _timer.Start();
        }
        private void User_List_Click(object sender, RoutedEventArgs e)
        {
           new AdminLoginWindow().Show();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            new EngineerLoginWindow().Show();
        }
       



    }
}