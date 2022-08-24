using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace RZFileExplorer.Converters {
    public class ConverterChain : IValueConverter {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<IValueConverter> Converters { get; } = new List<IValueConverter>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            foreach (IValueConverter converter in this.Converters) {
                value = converter.Convert(value, targetType, parameter, culture);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            foreach (IValueConverter converter in this.Converters) {
                value = converter.ConvertBack(value, targetType, parameter, culture);
            }

            return value;
        }
    }
}