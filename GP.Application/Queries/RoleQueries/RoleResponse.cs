using GP.Application.Queries.PermissionQueries;

namespace GP.Application.Queries.RoleQueries
{
    public class RoleResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public List<PermissionCategoryRelationResponse> Permissions { get; set; } = new();
        public bool IsEditable { get; set; }
    }
}
