using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using HandyControl.Data;

namespace BiuBiuWpfClient.Model
{
    public class MessageLocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Style)Application.Current.TryFindResource(((TypeLocalMessageLocation)value).ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}