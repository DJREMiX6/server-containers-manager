using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServerContainerManager.API.Json
{
    /// <summary>
    /// Ensures DateTime values are always serialized as ISO 8601 UTC ("Z" suffix)
    /// and deserialized as DateTimeKind.Utc regardless of the offset in the input.
    /// </summary>
    internal sealed class JsonUtcDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetDateTime();
            // Always treat incoming dates as UTC regardless of offset or no-offset input
            return value.Kind == DateTimeKind.Utc
                ? value
                : DateTime.SpecifyKind(value.ToUniversalTime(), DateTimeKind.Utc);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Always write as UTC — produces "2025-03-23T10:30:00Z"
            writer.WriteStringValue(
                value.Kind == DateTimeKind.Utc
                    ? value
                    : value.ToUniversalTime());
        }
    }
}
