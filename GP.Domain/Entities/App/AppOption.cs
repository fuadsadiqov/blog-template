using GP.Domain.Common.Configurations;
using GP.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Domain.Entities.App
{
    public class AppOption : Entity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        public bool IsOtpRequired { get; set; } = true;
        public bool IsPinRequired { get; set; }

    }
}
