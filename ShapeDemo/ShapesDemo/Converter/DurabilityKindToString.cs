using OpenDDSharp.DDS;
using System;
using System.Windows.Data;
using System.Globalization;

namespace OpenDDSharp.ShapesDemo.Converter
{
    [ValueConversion(typeof(DurabilityQosPolicyKind), typeof(string))]
    public class DurabilityKindToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            if (value == null)
                throw new ArgumentNullException("value");

            if (!(value is DurabilityQosPolicyKind))
                throw new InvalidCastException("The value must be a DurabilityQosPolicyKind type");

            DurabilityQosPolicyKind durability = (DurabilityQosPolicyKind)value;
            return durability == DurabilityQosPolicyKind.TransientLocalDurabilityQos ? "Transient Local" : value.ToString().Replace("DurabilityQos", "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return Enum.Parse(typeof(DurabilityQosPolicyKind), value.ToString().Replace(" ", "").ToString() + "DurabilityQos");            
        }
    }
}
