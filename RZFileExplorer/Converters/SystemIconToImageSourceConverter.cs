using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using RZFileExplorer.Icons;

namespace RZFileExplorer.Converters {
    public class SystemIconToImageSourceConverter : IValueConverter {
        public bool UseSmallIcon { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null || value == DependencyProperty.UnsetValue) {
                return value;
            }

            if (value is CSIDL icon) {
                try {
                    return ShellEx.GetBitmapSourceForSystemIcon(this.UseSmallIcon, icon);
                }
                catch {
                    return null;
                }
            }
            else {
                return $"[DEBUG_ERR_NOT_CSIDL: {value.GetType()} -> {value}]";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}