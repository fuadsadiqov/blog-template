using GP.Core.Enums.Enitity;
using GP.Core.Enums;
using GP.Core.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GP.Application.Queries.UserQueries
{
    public class UserInfoResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string WatermarkCode { get; set; }
        [Localize(nameof(Fullname), LanguageEnum.En)]
        public string FullnameEn { get; set; }
        [Localize(nameof(Fullname), LanguageEnum.Az)]
        public string FullnameAz { get; set; }
        public string Fullname { get; set; }
        public string Position { get; set; }
        [Localize(nameof(Position), LanguageEnum.Az)]
        public string PositionAz { get; set; }
        [Localize(nameof(Position), LanguageEnum.En)]
        public string PositionEn { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public RecordStatusEnum Status { get; set; }
        public bool IsLocked { get; set; } = false;
        public UserTypeEnum UserType { get; set; }
        public DateTime DateCreated { get; set; }
        public bool CanAccessOutOfDomain { get; set; }
    }
}
