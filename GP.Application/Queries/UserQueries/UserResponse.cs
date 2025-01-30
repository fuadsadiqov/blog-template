using GP.Application.Queries.PermissionQueries;
using GP.Application.Queries.RoleQueries;
using GP.Core.Enums.Enitity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GP.Application.Queries.UserQueries
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public RecordStatusEnum Status { get; set; }
        public UserTypeEnum UserType { get; set; }
        public List<RoleResponse> Roles { get; set; }
        public List<PermissionResponse> DirectivePermissions { get; set; }
        public DateTime DateCreated { get; set; }
        public bool CanAccessOutOfDomain { get; set; }
    }
}
