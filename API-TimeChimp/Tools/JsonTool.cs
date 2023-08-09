using Newtonsoft.Json;

namespace Api.Devion.Tools
{
    public static class JsonTool
    {
        //convert to object
        public static T ConvertTo<T>(String json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        //convert to string without null values
        public static String ConvertFrom(Object? obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        //convert to string with null values
        public static String ConvertFromWithNullValues(Object? obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}