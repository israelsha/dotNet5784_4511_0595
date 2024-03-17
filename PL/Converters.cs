﻿using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using PL.Task;

namespace PL
{
    /// <summary>
    /// This class converts the ID value to content (either "Add" or "Update").
    /// </summary>
    class ConvertIdToContent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 0 ? "Add" : "Update";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// This class converts the ID value to a boolean value indicating whether the control should be enabled.
    /// </summary>
    public class IdToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 0 ? true : false;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// This class converts the ID value to Visibility, collapsing if the value is 0 and visible otherwise.
    /// </summary>
    public class AddOrUpdateConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// This class converts the presence of schedule to Visibility, making it visible if there's no schedule and collapsed otherwise.
    /// </summary>
    public class HasScheduleConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (DateTime?)value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// This class converts the ID value to Visibility, making it visible if the value is 0 and collapsed otherwise.
    /// </summary>
    public class HasAllredyTaskConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

