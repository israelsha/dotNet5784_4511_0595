using PL.Engineer;
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

namespace PL
{

    /// <summary>
    /// Interaction logic for EngineerLoginWindow.xaml
    /// </summary>
    public partial class EngineerLoginWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerLoginWindow()
        {
            InitializeComponent();
        }

        private string EngineerName = string.Empty;
        private int id = 0;
        private void TextBox_UserName(object sender, TextChangedEventArgs e)
        {
            EngineerName = ((TextBox)sender).Text;  // now EngineerName contains the user name
        }
       
        
        private void TextBox_Id(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (int.TryParse(textBox.Text, out int result))
                id = result;
            else
            {
                MessageBox.Show("User does not exist");
                return;
            }

        }

        private void EngineerLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //BO.Engineer? engineer = s_bl.Engineer.Read(id);
                //if (engineer != null && engineer.Name == EngineerName)   //check if this is engineer
                //{
                //    new EngineerViewWindow().Show();
                //    Close();
                //}
                //else MessageBox.Show("User does not exist");


                new EngineerViewWindow().Show();
                Close();
            }
            catch
            {
                MessageBox.Show("User does not exist");
            }
        }
    }
}
