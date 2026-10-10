using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CeeVee.Converters
{
    public class SectionVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Boolean visibility
            if (value is bool)
            {
                return (bool)value? Visibility.Visible : Visibility.Collapsed;
            }

            // String visibility
            if (value is string)
            {
                return string.IsNullOrWhiteSpace(value as string)? Visibility.Collapsed : Visibility.Visible;
            }

            // Null / empty
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}