using System;
using System.Globalization;
using System.Windows.Data;

namespace OpenDDSharp.ShapesDemo.Converter
{
    [ValueConversion(typeof(int), typeof(int))]
    public class ConcentricSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int size = (int)value;
            return 2 * size / 5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
