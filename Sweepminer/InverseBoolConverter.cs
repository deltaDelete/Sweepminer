using System;
using System.Globalization;
using System.Windows.Data;

namespace Sweepminer; 

public class InverseBoolConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return (bool)value switch {
            true => false,
            false => true
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return (bool)value switch {
            true => false,
            false => true
        };
    }
}