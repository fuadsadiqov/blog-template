namespace GP.Application.Commands.RoleCommands.SetRoleUser
{
    public class SetRoleUserRequest
    {
        public List<string> UserIds { get; set; }
        public string RoleId { get; set; }
    }
}
