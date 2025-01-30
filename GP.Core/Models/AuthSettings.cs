namespace GP.Core.Models
{
    public class AuthSettings
    {
        public int LockoutExpiry { get; set; }
        public int MaxFailedAccessAttempts { get; set; }
        public int OtpExpiry { get; set; }
    }
}
