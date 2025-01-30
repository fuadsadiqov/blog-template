using AutoMapper;
using GP.Application.Commands.RoleCommands.CreateRole;
using GP.Application.Commands.RoleCommands.UpdateRole;
using GP.Application.Queries.RoleQueries;
using GP.Domain.Entities.Identity;

namespace GP.Application.Mapping
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            #region Role

            CreateMap<Role, RoleResponse>()
                .ForMember(c => c.Permissions,
                    opt => opt.MapFrom(m => m.PermissionCategory.Select(c => c.PermissionCategoryPermission)));

            CreateMap<CreateRoleRequest, Role>();
            CreateMap<UpdateRoleRequest, Role>();

            #endregion
        }
    }
}
