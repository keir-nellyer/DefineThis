using Newtonsoft.Json;

namespace DefineThis.Serialization
{
    public interface IJsonSerializerFactory
    {
        JsonSerializer Create();
    }
}