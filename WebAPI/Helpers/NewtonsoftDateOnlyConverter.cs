namespace WebAPI.Helpers
{
    using Newtonsoft.Json;
    using System;

    public class NewtonsoftDateOnlyConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is DateOnly date)
            {
                writer.WriteValue(date.ToString("yyyy-MM-dd"));
            }
            else
            {
                writer.WriteNull();
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String && reader.Value != null)
            {
                if (DateOnly.TryParseExact((string)reader.Value, "yyyy-MM-dd", out var result))
                    return result;
            }

            return default(DateOnly); // או throw לפי הצורך
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateOnly);
        }
    }

}
