using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BiuBiuWpfClient.Model
{
    public class MessageTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Style)Application.Current.TryFindResource(((ChatInfoType)value).ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}