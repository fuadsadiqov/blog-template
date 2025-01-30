namespace GP.Application.Commands.UserCommands.SetUserPermission
{
    public class SetUserPermissionRequest
    {
        public string UserId { get; set; }
        public List<string> PermissionIds { get; set; }
    }
}
