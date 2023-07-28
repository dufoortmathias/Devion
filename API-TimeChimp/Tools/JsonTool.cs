using Newtonsoft.Json;

namespace Api.Devion.Tools
{
    public static class JsonTool
    {
        public static T ConvertTo<T>(String json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static String ConvertFrom(Object? obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static String ConvertFromWithNullValues(Object? obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
