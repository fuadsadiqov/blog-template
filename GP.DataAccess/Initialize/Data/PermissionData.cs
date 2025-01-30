using GP.Core.Enums.Enitity;
using GP.Domain.Entities.Identity;

namespace GP.DataAccess.Initialize.Data
{
    public static partial class InitializeData
    {

        private static string GetPermissionVisibleName(string permission)
        {
            var visibleName = permission switch
            {
                nameof(PermissionEnum.Add) => "Add",
                nameof(PermissionEnum.Edit) => "Edit",
                nameof(PermissionEnum.Delete) => "Delete",
                nameof(PermissionEnum.List) => "List",
                nameof(PermissionEnum.Open) => "Open",
                nameof(PermissionEnum.Set) => "Set",
                nameof(PermissionEnum.SetAccess) => "Set Access",
                _ => ""
            };
            return visibleName;
        }
        public static List<Permission> BuildPermissionsList()
        {
            var list = new List<Permission>();
            var directivePermissions = new List<string>()
            {
                nameof(PermissionEnum.Admin),nameof( PermissionEnum.Moderator)
            };
            foreach (var permission in Enum.GetNames(typeof(PermissionEnum)))
            {
                list.Add(new Permission()
                {
                    Label = permission,
                    VisibleLabel = GetPermissionVisibleName(permission),
                    DateCreated = DateTime.Now,
                    Description = "",
                    Status = RecordStatusEnum.Active,
                    IsDirective = directivePermissions.Any(c => c == permission)
                });
            }

            return list;
        }
    }
}
