using GP.DataAccess.Repository;
using GP.DataAccess.Repository.SmsSenderServiceLogRepository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text;

namespace GP.Infrastructure.Services.SmsService
{
    public class SmsSenderService
    {
        private readonly SmsServiceSettings _smsServiceSettings;
        private readonly ISmsSenderServiceLogRepository _smsSenderServiceLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SmsSenderService(IOptions<SmsServiceSettings> smsServiceSettings,
            ISmsSenderServiceLogRepository smsSenderServiceLogRepository, IUnitOfWork unitOfWork)
        {
            _smsSenderServiceLogRepository = smsSenderServiceLogRepository;
            _smsServiceSettings = smsServiceSettings.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task SendSmsAsync(string userId, string number, string message)
        {
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;
            if (isDevelopment)
            {
                await SendSmsAsyncInDevelopmentAsync(userId, number, message);
            }
            else
            {
                var uri = $"{_smsServiceSettings.EndPoint}?user={_smsServiceSettings.UserName}&" +
                          $"password={_smsServiceSettings.Password}&gsm={number}&" +
                          $"from={_smsServiceSettings.SenderName}&text={message}";
                Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using HttpClient client = new HttpClient();

                HttpResponseMessage result = client.GetAsync(uri).Result;
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var responseData = ServiceResponseData.Deserialize(content);
                    var logData = responseData.SerializeToSmsSenderServiceLog();
                    logData.UserId = userId;
                    logData.Phone = number;
                    logData.SmsText = message;
                    await _smsSenderServiceLogRepository.AddAsync(logData);
                    await _unitOfWork.CompleteAsync();
                }
            }
        }

        public async Task SendConfirmationSmsAsync(string userId, string number, string confirmationCode, int activationDuration)
        {
            var message = @$"Sizin təsdiq kodunuz: {confirmationCode}%0aKod {activationDuration} dəqiqə ərzində aktiv olacaqdır";
            await SendSmsAsync(userId, number, message);
        }

        private async Task SendSmsAsyncInDevelopmentAsync(string userId, string number, string message)
        {
            var responseText = "errno=100&errtext=OK&message_id=558143397&charge=1&balance=49544";
            var responseData = ServiceResponseData.Deserialize(responseText);
            var logData = responseData.SerializeToSmsSenderServiceLog();
            logData.UserId = userId;
            logData.Phone = number;
            logData.SmsText = message;
            await _smsSenderServiceLogRepository.AddAsync(logData);
            await _unitOfWork.CompleteAsync();
        }
    }
}
