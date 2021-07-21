﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BiuBiuWpfClient.Model
{
    public class NoReadNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int NoReadNumber = (int)value;
            if (NoReadNumber != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}