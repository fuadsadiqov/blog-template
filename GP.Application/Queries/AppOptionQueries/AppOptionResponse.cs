using GP.Application.Queries.UserQueries;

namespace GP.Application.Queries.AppOptionQueries
{
    public class AppOptionResponse
    {
        public int Id { get; set; }
        public UserInfoResponse User { get; set; }
        public bool IsOtpRequired { get; set; } = true;
        public bool IsPinRequired { get; set; }
    }
}
