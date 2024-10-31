using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ClientChat.Converters
{
    public class BoolToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue)
            {
                return Brushes.Fuchsia; // Цвет фона для исходящих сообщений
            }
            else
            {
                return Brushes.LightBlue; // Цвет фона для входящих сообщений
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
