using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace OpenDDSharp.ShapesDemo.Converter
{
    [ValueConversion(typeof(bool), typeof(SolidColorBrush))]
    public class ConcentricColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isPublished = (bool)value;
            if (isPublished)
                return new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
            else
                return new SolidColorBrush(Color.FromRgb(0x44, 0x44, 0x44));            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
