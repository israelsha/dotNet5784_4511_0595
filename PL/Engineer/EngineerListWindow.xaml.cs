using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

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

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new EngineerWindow(0).ShowDialog();

    }
}
