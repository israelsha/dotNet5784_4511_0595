using System.Windows;
using System.Windows.Controls;


namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = s_bl?.Engineer.ReadAll()!;
    }
   
    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>) GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }

    }   

    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));
    public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;

    private void Level_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EngineerList = (Level == BO.EngineerExperience.None) ?
        s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => (int)item.Level==(int)Level)!;
    }

    private void AddEngineer_Button(object sender, RoutedEventArgs e)
    {
       Close();
        new EngineerWindow(0).ShowDialog();

    }

    private void UpdateEngineer_Button(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
        if (engineer != null)
        {
            Close();
            new EngineerWindow(engineer!.Id).ShowDialog();
        }
    }

    private void Home_Click(object sender, RoutedEventArgs e)
    {
        Close();
        new AdminViewWindow().ShowDialog();
    }
}
