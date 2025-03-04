using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenDDSharp.Marshaller.Json
{
    /// <summary>
    /// Float JSON converter.
    /// </summary>
    public class FloatJsonConverter : JsonConverter<float>
    {
        /// <inheritdoc/>
        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetSingle();
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(value.ToString("0.0###########################", CultureInfo.InvariantCulture));
        }
    }
}
