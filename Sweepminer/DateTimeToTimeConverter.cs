using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sweepminer;

public class DateTimeToTimeConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is DateTime time) {
            return time.ToString("HH:mm:ss");
        }

        if (value is TimeSpan timeSpan) {
            return timeSpan.ToString("g")[..^3];
        }

        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return DependencyProperty.UnsetValue;
    }
}