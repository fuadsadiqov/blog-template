using GP.Core.Attributes;
using GP.Core.Configurations.Entity;
using GP.Core.Constants;
using GP.Core.Enums;
using GP.Core.Enums.Enitity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GP.Domain.Entities.Common;

namespace GP.Domain.Entities.Identity
{
    public class User : IdentityUser<string>, IEntity
    {
        public UserTypeEnum UserType { get; set; } = UserTypeEnum.LdapUser;
        public string? Name { get; set; }
        public string? Surname { get; set; }
        [NotMapped]
        public string? FullName { get; set; }
        [StringLength(StringLengthConstants.Small)]
        [Localize(nameof(FullName), LanguageEnum.En)]
        public string? FullNameEn { get; set; }
        [StringLength(StringLengthConstants.Small)]
        [Localize(nameof(FullName), LanguageEnum.Az)]
        public string? FullNameAz { get; set; }
        public RecordStatusEnum Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string? Uuid { get; set; }
        public int OtpSentCount { get; set; }
        public DateTime? LastAccessFailedAttempt { get; set; }

        [InverseProperty("User")]
        public ICollection<UserJwt> Jwts { get; set; } = [];
        [InverseProperty("CreatedBy")]
        public ICollection<UserJwt> CreatedJwts { get; set; } = [];
        [InverseProperty("ModifiedBy")]
        public ICollection<UserJwt> ModifiedJwts { get; set; } = [];


        [InverseProperty("User")]
        public ICollection<UserRole> Roles { get; set; } = [];
        [InverseProperty("CreatedBy")]
        public ICollection<UserRole> CreatedRoles { get; set; } = [];
        [InverseProperty("ModifiedBy")]
        public ICollection<UserRole> ModifiedRoles { get; set; } = [];

        [InverseProperty("User")]
        public ICollection<UserPermission> DirectivePermissions { get; set; } = [];

        [InverseProperty("User")]
        public ICollection<EmailConfirmationRequest> EmailConfirmationRequests { get; set; } = [];
        [InverseProperty("CreatedBy")]
        public ICollection<EmailConfirmationRequest> CreatedEmailConfirmationRequests { get; set; } = [];
        [InverseProperty("ModifiedBy")]
        public ICollection<EmailConfirmationRequest> ModifiedEmailConfirmationRequests { get; set; } = [];


        [InverseProperty("User")]
        public ICollection<PasswordResetRequest> PasswordResetRequests { get; set; } = [];
        [InverseProperty("CreatedBy")]
        public ICollection<PasswordResetRequest> CreatedPasswordResetRequests { get; set; } = [];
        [InverseProperty("ModifiedBy")]
        public ICollection<PasswordResetRequest> ModifiedPasswordResetRequests { get; set; } = [];
        public User? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public string? CreatedById { get; set; }

        public User? ModifiedBy { get; set; }
        [ForeignKey("ModifiedBy")]
        public string? ModifiedById { get; set; }
        public bool CanAccessOutOfDomain { get; set; } = false;
        
        [InverseProperty("User")] 
        public ICollection<Review> Reviews { get; set; }
    }
}
