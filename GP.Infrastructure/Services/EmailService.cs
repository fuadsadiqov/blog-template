using GP.Core.Models;
using GP.DataAccess.Repository.UserRepository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net.Mime;

namespace GP.Infrastructure.Services
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public IEnumerable<string> Cc { get; set; } = new List<string>();
        public List<string> Attachments { get; set; } = new List<string>();
        public string FromEmail { get; set; }
        public string DisplayName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; } = true;
    }

    public class EmailService
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly SmtpSettings _settings;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly EmailTemplatingService _emailTemplatingService;

        public EmailService(IOptions<SmtpSettings> options, AuthService authService, IUserRepository userRepository,
            IHostEnvironment hostEnvironment, EmailTemplatingService emailTemplatingService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _settings = options.Value;
            _hostEnvironment = hostEnvironment;
            _emailTemplatingService = emailTemplatingService;
        }

        public async Task SendAsync(MailRequest mailRequest)
        {
            try
            {
                var smtpClient = new SmtpClient()
                {
                    Host = _settings.Host,
                    //Port = 25,
                    //EnableSsl = true,
                    //UseDefaultCredentials = true
                };

                var msg = new MailMessage(_settings.FromMail, mailRequest.ToEmail);
                if (mailRequest.Cc.Any())
                {
                    foreach (var item in mailRequest.Cc)
                    {
                        msg.CC.Add(new MailAddress(item));
                    }
                }

                msg.From = new MailAddress(_settings.FromMail, _settings.DisplayName);

                msg.Subject = mailRequest.Subject;
                msg.Body = mailRequest.Body;
                msg.IsBodyHtml = mailRequest.IsBodyHtml;

                if (mailRequest.Attachments.Any())
                {
                    foreach (var attachmentPath in mailRequest.Attachments)
                    {
                        Attachment attachment = new Attachment(attachmentPath, MediaTypeNames.Application.Octet);
                        msg.Attachments.Add(attachment);
                    }
                }

                await smtpClient.SendMailAsync(msg);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

            }
        }

        public async Task TestEmail()
        {
            await SendAsync(new MailRequest()
            {
                FromEmail = "noreplay@cookbook.az",
                ToEmail = "azer.imanguliyev@gmail.com",
                //Cc = ccEmails,
                DisplayName = "test",
                Subject = "Just test email",
                Body = "it is for testing purpose"
            });
        }

        public async Task UserRegistrationEmail(string toEmail, string hash)
        {
            var body = _emailTemplatingService.GenerateUserActivateEmailBody(new ResetPasswordEmailTemplateModel
            {
                Url = hash
            });

            await SendAsync(new MailRequest()
            {
                FromEmail = _settings.FromMail,
                ToEmail = toEmail,
                DisplayName = _settings.DisplayName,
                Subject = "Hesabın aktivləşdirilməsi",
                Body = body
            });
        }

        public async Task PasswordChangeEmail(string toEmail, string hash)
        {
            var body = _emailTemplatingService.GenerateChangePasswordChangeEmailBody(new ResetPasswordEmailTemplateModel
            {
                Url = hash
            });

            await SendAsync(new MailRequest()
            {
                FromEmail = _settings.FromMail,
                ToEmail = toEmail,
                DisplayName = _settings.DisplayName,
                Subject = "Şifrənin yenilənməsi",
                Body = body
            });
        }
    }
}
