﻿using PL.Task;
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
    /// Interaction logic for GanttchartWindow.xaml
    /// </summary>
    public partial class GanttchartWindow : Window
    {
        public GanttchartWindow()
        {
            InitializeComponent();
        }
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }
        public static readonly DependencyProperty TaskListProperty =
       DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(GanttchartWindow), new PropertyMetadata(s_bl.Task.ReadAll()));

    }
}
