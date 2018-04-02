using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace OpenDDSharp.ShapesDemo.Converter
{
    [ValueConversion(typeof(int), typeof(PointCollection))]
    public class TrianglePointsConverter : IValueConverter
    {        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int size = (int)value;                

                PointCollection pc = new PointCollection
                {
                    new Point(size / 2d, 0d),
                    new Point(0d, size),
                    new Point(size, size)
                };

                return pc;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
