using System.Windows;
using System.Windows.Controls;

namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerWindow.xaml
/// </summary>
public partial class EngineerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // Dependency property to bind the current engineer
    public BO.Engineer CurrentEngineer
    {
        get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
        set { SetValue(CurrentEngineerProperty, value); }
    }

    public static readonly DependencyProperty CurrentEngineerProperty =
        DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

    // Constructor for EngineerWindow
    public EngineerWindow(int Id = 0)
    {
        InitializeComponent();
        // Initialize CurrentEngineer based on the provided Id
        if (Id == 0)
        {
            CurrentEngineer = new BO.Engineer
            {
                Id = 0,
                Name = "",
                Email = "",
                Cost = 0,
                Level = BO.EngineerExperience.None,
                Task = null
            };
        }
        else
            CurrentEngineer = s_bl.Engineer.Read(Id);
    }

    int Id = 0;
    public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    /// <summary>
    /// Handles the button click event to add or update engineer values
    /// </summary>
    private void AddOrUpdate_Button(object sender, RoutedEventArgs e)
    {
        // Create a new engineer object with the values from the UI
        BO.Engineer engineer = new BO.Engineer
        {
            Id = CurrentEngineer.Id,
            Name = CurrentEngineer.Name,
            Level = CurrentEngineer.Level,
            Cost = CurrentEngineer.Cost,
            Task = null,
            Email = CurrentEngineer.Email
        };

        // Determine if the button is for adding or updating engineer
        string? buttonText = (sender as Button)?.Content?.ToString();
        try
        {
            if (buttonText == "Add")
            {
                // Call the business logic layer to create a new engineer
                s_bl.Engineer.Create(engineer!);
                MessageBox.Show("The engineer was successfully added");
            }
            else if (buttonText == "Update")
            {
                // Call the business logic layer to update the existing engineer
                s_bl.Engineer.Update(engineer!);
                MessageBox.Show("The engineer was successfully updated");
            }
            // Close the current window and open the EngineerListWindow
            Close();
            new EngineerListWindow().Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    // Handles the click event of the home button
    private void Home_Click(object sender, RoutedEventArgs e)
    {
        Close();
        new AdminViewWindow().Show();
    }
}
