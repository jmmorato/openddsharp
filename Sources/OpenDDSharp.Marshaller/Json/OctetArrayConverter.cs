using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenDDSharp.Marshaller.Json
{
    /// <summary>
    /// Octet array converter.
    /// </summary>
    public sealed class OctetArrayConverter : JsonConverter<byte[]>
    {
        /// <inheritdoc/>
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var bytes = new List<byte>();
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                return Array.Empty<byte>();
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.Number)
                {
                    bytes.Add(reader.GetByte());
                }
            }

            return !bytes.Any() ? Array.Empty<byte>() : bytes.ToArray();
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartArray();
            foreach (var b in value)
            {
                writer.WriteNumberValue(b);
            }

            writer.WriteEndArray();
        }
    }
}
