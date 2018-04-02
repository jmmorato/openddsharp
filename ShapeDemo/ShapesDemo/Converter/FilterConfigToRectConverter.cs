using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using OpenDDSharp.ShapesDemo.Model;

namespace OpenDDSharp.ShapesDemo.Converter
{
    [ValueConversion(typeof(ReaderFilterConfig), typeof(Rect))]
    public class FilterConfigToRectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReaderFilterConfig)
            {
                ReaderFilterConfig config = value as ReaderFilterConfig;
                return new Rect(new Point(config.X0, config.Y0), new Point(config.X1, config.Y1));
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
