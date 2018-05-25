using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LetterApp.Core.Serialization
{
    public class JsonConvert<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            if (existingValue == null)
            {
                if (serializer.ContractResolver.ResolveContract(objectType).DefaultCreator != null)
                    existingValue = serializer.ContractResolver.ResolveContract(objectType).DefaultCreator();
                else
                    existingValue = default(T);
            }

            var jToken = jObject.SelectToken("data");

            if (typeof(T) == typeof(String))
                return jToken.ToString();

            var jsonReader = jToken != null ? jToken.CreateReader() : jObject.CreateReader();

            serializer.Populate(jsonReader, existingValue);

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {}
    }
}
