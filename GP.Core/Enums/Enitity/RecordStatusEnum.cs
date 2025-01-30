using System.Runtime.Serialization;

namespace GP.Core.Enums.Enitity
{
    public enum RecordStatusEnum : byte
    {
        [EnumMember(Value = "active")]
        Active = 1,
        [EnumMember(Value = "passive")]
        Passive,
        [EnumMember(Value = "deleted")]
        Deleted,
    }
}
