using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenDDSharp.Marshaller.Json
{
    public class FloatJsonConverter : JsonConverter<float>
    {
        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetSingle();
        }

        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(value.ToString("0.0###########################"));
        }
    }
}
