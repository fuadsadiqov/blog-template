using GP.Domain.Common.Configurations;
using GP.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Domain.Entities.Audit
{
    public class SmsSenderServiceLog : Entity
    {
        public Guid Id { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public User User { get; set; }
        public string Phone { get; set; }
        public string SmsText { get; set; }
        public string MessageId { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseText { get; set; }
        public string Balance { get; set; }
    }
}
