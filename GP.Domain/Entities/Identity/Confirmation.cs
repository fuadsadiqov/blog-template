using GP.Domain.Common.Configurations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GP.Core.Enums;

namespace GP.Domain.Entities.Identity
{
    public class Confirmation : Entity
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }
        public string? ConfirmationHash { get; set; }
        public DateTime Expiration { get; set; }
        public ConfirmationTypeEnum Type { get; set; }
    }
}
