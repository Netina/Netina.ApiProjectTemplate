using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Common.Extensions;

public static class NewtonJsonExtensions
{
    public static JsonSerializerSettings CamelCaseSerialize =>
        new()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        };
}