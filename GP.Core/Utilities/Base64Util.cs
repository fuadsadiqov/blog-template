using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;

namespace GP.Core.Utilities
{
    public static class Base64Util
    {
        public static string ObjectToJsonString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T StringToObject<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        public static string ToBase64(this object obj)
        {
            string json = JsonConvert.SerializeObject(obj);

            byte[] bytes = Encoding.Default.GetBytes(json);

            return Convert.ToBase64String(bytes);
        }
        public static T FromBase64<T>(this string base64Text)
        {
            byte[] bytes = Convert.FromBase64String(base64Text);

            string json = Encoding.Default.GetString(bytes);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
