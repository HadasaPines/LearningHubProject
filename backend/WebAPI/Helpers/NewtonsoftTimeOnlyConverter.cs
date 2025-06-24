using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WebAPI.Helpers
{


    using Newtonsoft.Json;
    using System;

    public class NewtonsoftTimeOnlyConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is TimeOnly time)
            {
                writer.WriteValue(time.ToString("HH:mm"));
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
                if (TimeOnly.TryParseExact((string)reader.Value, "HH:mm", out var result))
                    return result;
            }

            return default(TimeOnly); // או throw לפי בחירה
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeOnly);
        }
    }




}
