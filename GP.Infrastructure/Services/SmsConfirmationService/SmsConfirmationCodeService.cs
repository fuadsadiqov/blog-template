using GP.Core.Enums.Enitity;
using GP.Core.Extensions;
using GP.Core.Models;
using GP.DataAccess.Repository;
using GP.DataAccess.Repository.UserSmsConfirmationRequestRepository;
using GP.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GP.Infrastructure.Services.SmsConfirmationService
{
    public class SmsConfirmationCodeService
    {
        private readonly IUserSmsConfirmationRequestRepository _userSmsConfirmationRequestRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthSettings _authSettings;

        public SmsConfirmationCodeService(IUserSmsConfirmationRequestRepository userSmsConfirmationRequestRepository,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AuthSettings> authSettings)
        {
            _userSmsConfirmationRequestRepository = userSmsConfirmationRequestRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _authSettings = authSettings.Value;
        }

        public async Task<string> GetConfirmationCode(string userId, string phone, SmsRequestTypeEnum smsRequestType)
        {
            var ipAddress = _httpContextAccessor.HttpContext?.GetRequestIp();
            var generatedToken = GenerateCode();
            var confirmationCodes = await
                 _userSmsConfirmationRequestRepository.
                    FindBy(x => x.UserId == userId &&
                            x.Phone == phone &&
                            x.IpAddress == ipAddress &&
                            x.SmsRequestTypeEnum == smsRequestType &&
                            x.ConfirmationStatus == SmsConfirmationStatusEnum.Created
                            ).OrderByDescending(x => x.DateCreated).
                    FirstOrDefaultAsync();
            if (confirmationCodes != null)
                confirmationCodes.ConfirmationStatus = SmsConfirmationStatusEnum.Fail;

            var data = new UserSmsConfirmationRequest()
            {
                UserId = userId,
                Phone = phone,
                IpAddress = ipAddress,
                Token = generatedToken,
                ExpireDate = DateTime.Now.AddMinutes(_authSettings.OtpExpiry),
                ConfirmationStatus = SmsConfirmationStatusEnum.Created,
                SmsRequestTypeEnum = smsRequestType
            };

            await _userSmsConfirmationRequestRepository.AddAsync(data);
            await _unitOfWork.CompleteAsync();

            return data.Token;
        }

        public async Task<bool> IsValidConfirmationCodeAsync(string userId, string phone, string code, SmsRequestTypeEnum smsRequestType, bool needApprove)
        {
            var ipAddress = _httpContextAccessor.HttpContext?.GetRequestIp();
            var confirmationCode =
                await _userSmsConfirmationRequestRepository.FindBy(x => x.UserId == userId &&
                                        x.Phone == phone &&
                                        x.IpAddress == ipAddress &&
                                        x.Token == code &&
                                        x.SmsRequestTypeEnum == smsRequestType &&
                                        x.ConfirmationStatus == SmsConfirmationStatusEnum.Created).
                            OrderByDescending(x => x.DateCreated).
                            FirstOrDefaultAsync();

            if (confirmationCode == null) return false;
            var dateTimeNow = DateTime.Now;
            var result = (dateTimeNow < confirmationCode.ExpireDate);
            if (result && needApprove)
            {
                confirmationCode.ConfirmationStatus = SmsConfirmationStatusEnum.Approved;
                await _unitOfWork.CompleteAsync();
            }

            return result;
        }

        #region private methods
        private static string GenerateCode()
        {
            Random rnd = new Random();
            int number = rnd.Next(1000, 10000);
            return $"{number}";
        }
        #endregion

    }
}
