using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using GP.Core.Models;
using GP.Domain.Common.Configurations;

namespace GP.Domain.Entities.Identity
{
    [Index("Token")]
    public class UserJwt : Entity
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime? RevokeDate { get; set; }
        public string? ReplacedByToken { get; set; }
        public string? ReasonRevoked { get; set; }
        [ForeignKey("Impersonator")]
        public string? ImpersonatorId { get; set; }
        public User? Impersonator { get; set; }

        public string? Domain { get; set; }
        public string? RememberMe { get; set; }
        public string? Application { get; set; }
        [ForeignKey("User")]
        [Required]
        public string? UserId { get; set; }
        public User? User { get; set; }
        [NotMapped]
        public bool IsExpired => DateTime.Now >= ExpireDate;
        [NotMapped]
        public bool IsRevoked => RevokeDate != null;
        [NotMapped]
        public bool IsActive => !IsRevoked && !IsExpired;

        public TokenClaim ToTokenClaim()
        {
            var tokenClaim = new TokenClaim();
            if (!string.IsNullOrEmpty(Application))
                tokenClaim.Application = new Claim(CustomClaimTypes.Application, Application);
            if (!string.IsNullOrEmpty(Domain))
                tokenClaim.Domain = new Claim(CustomClaimTypes.Domain, Domain);
            if (!string.IsNullOrEmpty(RememberMe))
                tokenClaim.RememberMe = new Claim(CustomClaimTypes.RememberMe, RememberMe);
            if (!string.IsNullOrEmpty(ImpersonatorId))
                tokenClaim.ImpersonatorId = new Claim(CustomClaimTypes.Impersonator, ImpersonatorId);
            if (User != null)
                tokenClaim.CanAccessOutOfDomain = new Claim(CustomClaimTypes.CanAccessOutOfDomain,
                User.CanAccessOutOfDomain.ToString());
            if (Impersonator != null)
                tokenClaim.ImpersonatorName = new Claim(CustomClaimTypes.ImpersonatorName, Impersonator.UserName!);

            return tokenClaim;
        }
    }
}
