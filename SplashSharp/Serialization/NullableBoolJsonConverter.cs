using System;
using Newtonsoft.Json;

namespace SplashSharp.Serialization
{
    public class NullableBoolConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var asBool = (bool?)value;

            if (asBool == null) writer.WriteNull();
            else
            {
                if (asBool.Value) writer.WriteValue(1);
                else writer.WriteValue(0);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return null;
            else if (reader.Value.ToString().Equals("1")) return true;
            else if (reader.Value.ToString().Equals("0")) return false;

            throw new JsonException($"Could not read value '{reader.Value.ToString()}' as nullable bool");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool?);
        }
    }
}
