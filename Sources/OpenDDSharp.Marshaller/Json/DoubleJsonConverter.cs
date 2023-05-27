using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenDDSharp.Marshaller.Json
{
    public class DoubleJsonConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetDouble();
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(value.ToString("0.0###########################"));
        }
    }
}
