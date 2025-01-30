using GP.Core.Extensions;
using GP.Domain.Entities.Audit;

namespace GP.Infrastructure.Services.SmsService
{
    class ServiceResponseData
    {
        public string Errno { get; set; }
        public string Errtext { get; set; }
        public string Balance { get; set; }
        public string MessageId { get; set; }
        public string Charge { get; set; }
        public static ServiceResponseData Deserialize(string content)
        {

            var dic = content.Split("&").Select(c => c.Split("="))
                .Select(e => new KeyValuePair<string, string>(e[0], e[1])).
                ToDictionary(m => m.Key.ToPascalCase(), m => m.Value);

            var objectData = GetObject<ServiceResponseData>(dic);

            return objectData;

            T GetObject<T>(Dictionary<string, string> dict)
            {
                Type type = typeof(T);
                var obj = Activator.CreateInstance(type);
                foreach (var kv in dict)
                {
                    type.GetProperty(kv.Key).SetValue(obj, kv.Value);
                }
                return (T)obj;
            }
        }
        public SmsSenderServiceLog SerializeToSmsSenderServiceLog()
        {
            return new SmsSenderServiceLog()
            {
                ResponseText = Errtext,
                ResponseCode = Errno,
                Balance = Balance,
                MessageId = MessageId
            };
        }
    }
}
