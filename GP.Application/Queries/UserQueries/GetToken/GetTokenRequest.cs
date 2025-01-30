using System.Security.Claims;

namespace GP.Application.Queries.UserQueries.GetToken
{
    public class GetTokenRequest
    {
        public string UserId { get; set; } = string.Empty;
        public List<Claim> Claims { get; set; } = new List<Claim>();
    }
}
