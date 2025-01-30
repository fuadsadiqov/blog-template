namespace GP.Application.Commands.RoleCommands.SetRolePermission
{
    public class SetRolePermissionRequest
    {
        public string RoleId { get; set; }
        public List<int> PermissionsIds { get; set; } = new();
    }
}
