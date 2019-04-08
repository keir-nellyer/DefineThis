using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DefineThis.Serialization
{
    public class JsonSerializerFactory : IJsonSerializerFactory
    {
        public JsonSerializer Create()
        {
            var jsonSerializer = new JsonSerializer();
            
            // Camel-case property names
            jsonSerializer.ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            
            // Pretty print
            jsonSerializer.Formatting = Formatting.Indented;
            
            return jsonSerializer;
        }
    }
}