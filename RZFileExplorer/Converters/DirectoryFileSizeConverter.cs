using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RZFileExplorer.Converters {
    public class DirectoryFileSizeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null || value == DependencyProperty.UnsetValue) {
                return value;
            }

            if (value is long size) {
                return size == -1 ? "[Calculate]" : size.ToString();
            }
            else {
                return $"[DEBUG_ERROR_NOT_FILE: {value.GetType()} -> {value}]";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}