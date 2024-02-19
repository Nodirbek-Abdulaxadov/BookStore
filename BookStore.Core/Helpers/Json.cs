using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BookStore.Core.Helpers;

public static class Json
{
    public static string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            );
    }
}