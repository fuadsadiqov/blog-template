using GP.Core.Enums.Enitity;
using GP.DataAccess.Repository.RoleRepository;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;

namespace GP.Application.Commands.UserCommands.SetUserRole
{
    public class SetUserRoleCommandHandler : ICommandHandler<SetUserRoleCommand, SetUserRoleResponse>
    {
        private readonly IUserRepository _repository;
        private readonly IRoleRepository _roleRepository;
        private readonly AuthService _authService;
        private readonly ExceptionService _exceptionService;

        public SetUserRoleCommandHandler(IUserRepository repository, IRoleRepository roleRepository, AuthService authService, ExceptionService exceptionService)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _authService = authService;
            _exceptionService = exceptionService;
        }

        public async Task<SetUserRoleResponse> Handle(SetUserRoleCommand command, CancellationToken cancellationToken)
        {
            var userId = _authService.GetAuthorizedUserId();
            var isAdmin = await _authService.IsAdminAsync();

            if (!isAdmin)
                command.Request.RoleIds = new List<string>();

            _repository.SetGlobalQueryFilterStatus(false);

            var user = await _repository.GetUserByIdAsync(command.Request.UserId);
            if (user == null)
                throw _exceptionService.RecordNotFoundException();

            var removedRoles = user.Roles.Where(f => !command.Request.RoleIds.Contains(f.RoleId)).ToList();
            foreach (var item in removedRoles)
            {
                user.Roles.Remove(item);
            }

            if (command.Request.RoleIds != null && command.Request.RoleIds.Any())
            {
                var alreadyExistRoles = user.Roles.Where(c => command.Request.RoleIds.Contains(c.RoleId)).ToList();
                foreach (var role in alreadyExistRoles)
                {
                    role.Status = RecordStatusEnum.Active;
                }

                var addedRoles = command.Request.RoleIds.Where(id => !alreadyExistRoles.Select(e => e.RoleId).Contains(id) && user.Roles.All(f => f.RoleId != id)).Select(c => new UserRole() { RoleId = c, DateCreated = DateTime.Now, Status = RecordStatusEnum.Active }).ToList();
                foreach (var item in addedRoles)
                {
                    user.Roles.Add(item);
                }
            }

            await _repository.UpdateAsync(user).ConfigureAwait(false);
            return new SetUserRoleResponse();
        }
    }
}
