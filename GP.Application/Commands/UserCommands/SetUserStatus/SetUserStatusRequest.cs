using GP.Core.Enums.Enitity;

namespace GP.Application.Commands.UserCommands.SetUserStatus
{
    public class SetUserStatusRequest
    {
        public string UserId { get; set; }
        public RecordStatusEnum Status { get; set; } = RecordStatusEnum.Active;
    }
}
