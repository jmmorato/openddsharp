using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace OpenDDSharp.ShapesDemo.Converter
{
    [ValueConversion(typeof(int), typeof(Thickness))]
    public class TriangleMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness(0, System.Convert.ToDouble(value) / 4, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
