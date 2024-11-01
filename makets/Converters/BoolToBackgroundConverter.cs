using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace makets.Converters
{
    public class BoolToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue)
            {
                // Цвет фона для исходящих сообщений (светло-фиолетовый)
                return new SolidColorBrush(Color.FromArgb(255, 200, 180, 250)); // Мягкий светло-фиолетовый
            }
            else
            {
                // Цвет фона для входящих сообщений (светлый синий)
                return new SolidColorBrush(Color.FromArgb(255, 191, 229, 240)); // Цвет для входящих сообщений
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
