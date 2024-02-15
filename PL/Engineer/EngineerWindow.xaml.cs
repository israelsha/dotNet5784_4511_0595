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
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));
        bool IsCreate = false;
        public EngineerWindow(int Id = 0)
        {
            InitializeComponent();

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
                IsCreate = true;
            }
            else
            {
                CurrentEngineer = s_bl.Engineer.Read(Id);
                IsCreate = false;
            }

        }

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        int Id = 0;
        




        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void SaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    int EngineerId = 0;
        //    try
        //    {
        //        // Check if the ID textbox contains 0, indicating a new entity
        //        if (EngineerId == 0)
        //        {
        //            // Add the new entity to the BL
        //            s_bl.Engineer.Create();
        //            MessageBox.Show("Entity added successfully!", "Add Entity", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //        else // Otherwise, if an existing entity is detected
        //        {
        //            // Update the existing entity in the BL
        //            s_bl.Engineer.Update(Engineer);
        //            MessageBox.Show("Entity updated successfully!", "Update Entity", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }

        //    // Close the window
        //    Close();
        //}
        //catch (Exception ex)
        //{
        //    // Catch exceptions and notify the user
        //    MessageBox.Show($"An error occurred during save: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
        // }
    }
}
