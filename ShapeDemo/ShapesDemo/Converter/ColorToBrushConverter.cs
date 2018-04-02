using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OpenDDSharp.ShapesDemo.Converter
{
    [ValueConversion(typeof(string), typeof(SolidColorBrush))]
    class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33));

            switch (value)
            {
                case "BLUE":
                    return new SolidColorBrush(Color.FromRgb(0x33, 0x66, 0x99));
                case "RED":
                    return new SolidColorBrush(Color.FromRgb(0xCC, 0x33, 0x33));
                case "GREEN":
                    return new SolidColorBrush(Color.FromRgb(0x99, 0xCC, 0x66));
                case "ORANGE":
                    return new SolidColorBrush(Color.FromRgb(0xFF, 0x99, 0x33));
                case "YELLOW":
                    return new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0x66));
                case "MAGENTA":
                    return new SolidColorBrush(Color.FromRgb(0xCC, 0x99, 0xCC));
                case "CYAN":
                    return new SolidColorBrush(Color.FromRgb(0x99, 0xCC, 0xFF));
                case "GRAY":
                    return new SolidColorBrush(Color.FromRgb(0x99, 0x99, 0x99));
                default:
                    return new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
