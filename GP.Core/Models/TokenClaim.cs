using System.Security.Claims;

namespace GP.Core.Models
{
    public class TokenClaim
    {
        public Claim Domain { get; set; }
        public Claim Application { get; set; }
        public Claim ImpersonatorId { get; set; }
        public Claim ImpersonatorName { get; set; }
        public Claim CanAccessOutOfDomain { get; set; }
        public Claim RememberMe { get; set; }

        public TokenClaim()
        {
            
        }  

        public TokenClaim(List<Claim> claims)
        {
            this.GenerateFromList(claims);
        }

        private void GenerateFromList(List<Claim> claims)
        {
            var applicationClaim = claims?.FirstOrDefault(c => c.Type == CustomClaimTypes.Application);
            var domainClaim = claims?.FirstOrDefault(c => c.Type == CustomClaimTypes.Domain);
            var rememberMeClaim = claims?.FirstOrDefault(c => c.Type == CustomClaimTypes.RememberMe);
            var impersonator = claims?.FirstOrDefault(c => c.Type == CustomClaimTypes.Impersonator);
            var ImpersonatorName = claims?.FirstOrDefault(c => c.Type == CustomClaimTypes.ImpersonatorName);
            var canAccessOutOfDomain = claims?.FirstOrDefault(c => c.Type == CustomClaimTypes.CanAccessOutOfDomain);
            this.Domain = domainClaim;
            this.Application = applicationClaim;
            this.ImpersonatorId = impersonator;
            this.ImpersonatorName = ImpersonatorName;
            this.CanAccessOutOfDomain = canAccessOutOfDomain;
            this.RememberMe = rememberMeClaim;
        }

        public List<Claim> ToList()
        {
            return new List<Claim>()
            {
                Domain,
                Application,
                ImpersonatorId,
                ImpersonatorName,
                CanAccessOutOfDomain,
                RememberMe
            };
        }

    }
}
