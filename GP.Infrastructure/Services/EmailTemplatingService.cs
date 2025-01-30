using GP.Core.Models;
using System.Text;

namespace GP.Infrastructure.Services
{
    public class EmailTemplatingService
    {
        public string GenerateUserActivateEmailBody(ResetPasswordEmailTemplateModel model)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            // Start building the HTML content
            htmlBuilder.Append("<!DOCTYPE html>");
            htmlBuilder.Append("<html>");
            htmlBuilder.Append("<head>");
            htmlBuilder.Append("<title> Hesabın aktivləşdirilməsi </title>");
            htmlBuilder.Append("<style>");
            htmlBuilder.Append("body { font-family: Arial, sans-serif; }");
            htmlBuilder.Append(".container { max-width: 600px; padding: 20px 0; }");
            htmlBuilder.Append(".header { font-size: 24px; font-weight: bold; margin-bottom: 10px; }");
            htmlBuilder.Append("</style>");
            htmlBuilder.Append("</head>");
            htmlBuilder.Append("<body>");
            htmlBuilder.Append("<div class='container'>");
            htmlBuilder.Append($"<div><b>Hesabınızı aktivləşdirmək üçün linkə daxil olun.</b> <a href='https://tmanager.aih.local/auth/change-password?confirmation={model.Url}'>Daxil olun</a></div>");
            htmlBuilder.Append("</div>");
            htmlBuilder.Append("</body>");
            htmlBuilder.Append("</html>");

            return htmlBuilder.ToString();
        }

        public string GenerateChangePasswordChangeEmailBody(ResetPasswordEmailTemplateModel model)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            // Start building the HTML content
            htmlBuilder.Append("<!DOCTYPE html>");
            htmlBuilder.Append("<html>");
            htmlBuilder.Append("<head>");
            htmlBuilder.Append("<title> Şifrənin yenilənməsi </title>");
            htmlBuilder.Append("<style>");
            htmlBuilder.Append("body { font-family: Arial, sans-serif; }");
            htmlBuilder.Append(".container { max-width: 600px; padding: 20px 0; }");
            htmlBuilder.Append(".header { font-size: 24px; font-weight: bold; margin-bottom: 10px; }");
            htmlBuilder.Append("</style>");
            htmlBuilder.Append("</head>");
            htmlBuilder.Append("<body>");
            htmlBuilder.Append("<div class='container'>");
            htmlBuilder.Append($"<div><b>Şifrənizi yeniləmək üçün linkə daxil olun.</b> <a href='https://tmanager.aih.local/auth/change-password?confirmation={model.Url}'>Daxil olun</a></div>");
            htmlBuilder.Append("</div>"); // Close container
            htmlBuilder.Append("</body>");
            htmlBuilder.Append("</html>");

            return htmlBuilder.ToString();
        }
    }
}
