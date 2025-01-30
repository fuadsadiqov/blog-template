using AutoMapper;
using GP.Application.Commands.UserCommands.CreateUser;
using GP.Application.Commands.UserCommands.UpdateUser;
using GP.Application.Queries.UserQueries;
using GP.Domain.Entities.Identity;

namespace AIH.ERP.Analytic.Application.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            #region User
            CreateMap<User, UserResponse>()
                .ForMember(er => er.Roles,
                    opt => opt.MapFrom(e => e.Roles.Select(c => c.Role)))
                .ForMember(er => er.DirectivePermissions,
                    opt => opt.MapFrom(e => e.DirectivePermissions.Select(c => c.Permission)))
                ;
            CreateMap<User, UserInfoResponse>()
                .ForMember(d => d.IsLocked,
                    opt =>
                    {
                        opt.MapFrom(src => src.LockoutEnd != null && src.LockoutEnd.Value > DateTime.Now);
                    });

            CreateMap<CreateUserRequest, User>()
                .ForMember(c => c.Roles, opt => opt.Ignore())
                .ForMember(c => c.DirectivePermissions, opt => opt.Ignore());
            CreateMap<UpdateUserRequest, User>()
                .ForMember(c => c.Roles, opt => opt.Ignore())
                .ForMember(c => c.DirectivePermissions, opt => opt.Ignore());
            #endregion
        }
    }
}
