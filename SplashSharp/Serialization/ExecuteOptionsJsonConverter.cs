using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using SplashSharp.Requests;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace SplashSharp.Serialization
{
    public class LuaScriptOptionsJsonConverter<T> : JsonConverter where T : BaseLuaScriptOptions
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is T typed))
            {
                writer.WriteNull();
            }
            else
            {
                var snakeCase = new SnakeCaseNamingStrategy();
                
                // TODO: this is not including properties from base objects.
                var allProperties = typed.GetType().GetProperties(BindingFlags.Public|BindingFlags.Instance);
                writer.WriteStartObject();
                
                foreach (var prop in allProperties
                    .Where(p => !p.Name.Equals(nameof(BaseLuaScriptOptions.InternalProperties)) && !p.Name.Equals("Item")))
                {
                    var propertyValue = prop.GetValue(typed);

                    if (propertyValue == null)
                    {
                        continue;
                    }

                    writer.WritePropertyName(snakeCase.GetPropertyName(prop.Name, false));
                    
                    // TODO: there must be a better way to match both dictionaries generically..
                    if (propertyValue is Dictionary<string, object> asDict)
                    {
                        writer.WriteStartObject();
                        foreach (var kvp in asDict)
                        {
                            writer.WritePropertyName(kvp.Key);
                            writer.WriteValue(kvp.Value);
                        }

                        writer.WriteEndObject();
                    }
                    else if (propertyValue is Dictionary<string, string> asDictStr)
                    {
                        writer.WriteStartObject();
                        foreach (var kvp in asDictStr)
                        {
                            writer.WritePropertyName(kvp.Key);
                            writer.WriteValue(kvp.Value);
                        }

                        writer.WriteEndObject();
                    }
                    else
                    {
                        writer.WriteValue(prop.GetValue(typed));
                    }
                    
                }
                foreach (var kvp in typed.InternalProperties)
                {
                    writer.WritePropertyName(kvp.Key);
                    writer.WriteValue(kvp.Value);
                }

                writer.WriteEndObject();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }
    }
}
