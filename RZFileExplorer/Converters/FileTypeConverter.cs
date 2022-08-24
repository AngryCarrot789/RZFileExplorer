using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using RZFileExplorer.Files;

namespace RZFileExplorer.Converters {
    [ValueConversion(typeof(BaseFileItemViewModel), typeof(string))]
    public class FileTypeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null || value == DependencyProperty.UnsetValue) {
                return value;
            }

            if (value is BaseFileItemViewModel file) {
                if (file.IsFile) {
                    return "File";
                }
                else if (file.IsDirectory) {
                    return "Directory";
                }
                else if (file.IsDrive) {
                    return "Drive";
                }
                else {
                    return "UNKNOWN";
                }
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