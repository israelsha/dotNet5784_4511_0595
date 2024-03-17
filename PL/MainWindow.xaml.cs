using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using PL.Engineer;
using PL.Task;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    // Accessing the Business Logic layer
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // Counters for adjusting date and time
    static int countHour = 0;
    static int countmonth = 0;

    // Property for displaying current date and time
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

    // Event handler for timer tick
    private void Timer_Tick(object sender, EventArgs e)
    {
        if (!_updatingDateTime)
            UpdateDateTime();
    }

    // Method to update date and time
    private void UpdateDateTime()
    {
        DateTime dt = DateTime.Now;
        dt = dt.AddHours(countHour);
        dt = dt.AddMonths(countmonth);
        CurrentDateTime = dt.ToString();
    }

    // Event for property changed
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool _updatingDateTime = false;

    // Event handlers for adjusting time
    private void add_hour_Click(object sender, RoutedEventArgs e)
    {
        countHour++;
        s_bl.addHour();
    }
    private void sub_hour_Click(object sender, RoutedEventArgs e)
    {
        countHour--;
        s_bl.subHour();
    }

    private void add_day_Click(object sender, RoutedEventArgs e)
    {
        countHour += 24;
        s_bl.addDay();
    }
    private void sub_day_Click(object sender, RoutedEventArgs e)
    {
        countHour -= 24;
        s_bl.subDay();
    }

    private void add_month_Click(object sender, RoutedEventArgs e)
    {
        countmonth++;   
        s_bl.addMonth();
    }
    private void sub_month_Click(object sender, RoutedEventArgs e)
    {
        countmonth--;
        s_bl.subMonth();
    }

    private void Reset_clock_Click(object sender, RoutedEventArgs e)
    {
        countHour = 0;
        countmonth = 0;
        s_bl.ResetClock();
    }

    private void UpdateTimer()
    {
        _timer.Stop();
        _timer.Start();
    }

    // Event handler for opening AdminLoginWindow
    private void Admin_Click(object sender, RoutedEventArgs e)
    {
        new AdminLoginWindow().Show();
    }

    // Event handler for opening EngineerLoginWindow
    private void Engineer_Click(object sender, RoutedEventArgs e)
    {
        new EngineerLoginWindow().Show();
    }

   
}
