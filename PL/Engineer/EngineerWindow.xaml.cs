using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        private void AddOrUpdate_Button(object sender, RoutedEventArgs e)
        {
            BO.Engineer engineer = new BO.Engineer
            {
                Id = CurrentEngineer.Id,
                Name = CurrentEngineer.Name,
                Level = CurrentEngineer.Level,  
                Cost= CurrentEngineer.Cost,
                Task = null,
                Email= CurrentEngineer.Email
            };

            string? buttonText = (sender as Button)?.Content?.ToString();
            try
            {
                if (buttonText == "Add")
                {
                    s_bl.Engineer.Create(engineer!);
                    MessageBox.Show("The engineer was successfully added");
                    Close();
                }
                else if (buttonText == "Update")
                {
                    s_bl.Engineer.Update(engineer!);
                    MessageBox.Show("The engineer was successfully updated");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
