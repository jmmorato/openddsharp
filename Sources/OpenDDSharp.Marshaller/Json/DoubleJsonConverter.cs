using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenDDSharp.Marshaller.Json
{
    /// <summary>
    /// Double JSON converter.
    /// </summary>
    public class DoubleJsonConverter : JsonConverter<double>
    {
        /// <inheritdoc/>
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetDouble();
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(value.ToString("0.0###########################"));
        }
    }
}
