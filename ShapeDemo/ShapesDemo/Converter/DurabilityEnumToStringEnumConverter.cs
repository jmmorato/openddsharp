using OpenDDSharp.DDS;
using System;
using System.Linq;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;

namespace OpenDDSharp.ShapesDemo.Converter
{
    [ValueConversion(typeof(IEnumerable<DurabilityQosPolicyKind>), typeof(IEnumerable<string>))]
    public class DurabilityEnumToStringEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            IEnumerable<DurabilityQosPolicyKind> list = value as IEnumerable<DurabilityQosPolicyKind>;

            return list.Select((e) => e == DurabilityQosPolicyKind.TransientLocalDurabilityQos ? "Transient Local" : e.ToString().Replace("DurabilityQos", "")).ToList();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            IEnumerable<string> list = value as IEnumerable<string>;
            return list.Select((e) => Enum.Parse(typeof(DurabilityQosPolicyKind), e.Replace(" ", "") + "DurabilityQos"));
        }
    }
}
