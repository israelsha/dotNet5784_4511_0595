using System.Windows;
using System.Windows.Controls;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        // Static reference to the business logic layer
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // Constructor
        public EngineerListWindow()
        {
            InitializeComponent();
            // Initialize the EngineerList property with all engineers
            EngineerList = s_bl?.Engineer.ReadAll()!;
        }

        // Property to hold the list of engineers
        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        // Dependency property for EngineerList
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        // Property to hold the selected engineer's experience level
        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;

        // Handles the selection changed event of the level ComboBox
        private void Level_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update the EngineerList based on the selected level
            EngineerList = (Level == BO.EngineerExperience.None) ?
                s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => (int)item.Level == (int)Level)!;
        }

        // Handles the click event of the Add Engineer button
        private void AddEngineer_Button(object sender, RoutedEventArgs e)
        {
            // Close the current window and open the EngineerWindow to add a new engineer
            Close();
            new EngineerWindow(0).ShowDialog();
        }

        // Handles the double click event of an engineer in the ListView
        private void UpdateEngineer_Button(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Get the selected engineer from the ListView
            BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
            // If an engineer is selected, close the current window and open the EngineerWindow to update the selected engineer
            if (engineer != null)
            {
                Close();
                new EngineerWindow(engineer!.Id).ShowDialog();
            }
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new AdminViewWindow().Show();
        }
    }
}
