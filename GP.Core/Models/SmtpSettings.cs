namespace GP.Core.Models
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public string DisplayName { get; set; }
        public string FromMail { get; set; }
        public string ToMail { get; set; }
        public int Port { get; set; }
    }
}
