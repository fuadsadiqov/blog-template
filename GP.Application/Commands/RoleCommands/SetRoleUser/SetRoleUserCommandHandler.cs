using GP.Core.Enums.Enitity;
using GP.DataAccess.Repository.RoleRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using MediatR;

namespace GP.Application.Commands.RoleCommands.SetRoleUser
{
    public class SetRoleUserCommandHandler : ICommandHandler<SetRoleUserCommand, SetRoleUserResponse>
    {
        private readonly IRoleRepository _repository;
        private readonly AuthService _authService;
        private readonly ExceptionService _exceptionService;
        private readonly IMediator _mediator;

        public SetRoleUserCommandHandler(IRoleRepository repository, AuthService authService, ExceptionService exceptionService, IMediator mediator)
        {
            _repository = repository;
            _authService = authService;
            _exceptionService = exceptionService;
            _mediator = mediator;
        }

        public async Task<SetRoleUserResponse> Handle(SetRoleUserCommand command, CancellationToken cancellationToken)
        {
            var userId = _authService.GetAuthorizedUserId();
            var havePermissionManagerAddPermission =
                await _authService.UserIsInPermissionAsync(userId, $"permissionmanager_{PermissionEnum.Add}");

            var haveRoleSetPermission =
                await _authService.UserIsInPermissionAsync(userId, $"role_{PermissionEnum.Set}");
            var haveRoleListPermission =
                await _authService.UserIsInPermissionAsync(userId, $"role_{PermissionEnum.List}");
            var isAdmin = await _authService.IsAdminAsync();

            if (!((haveRoleSetPermission && haveRoleListPermission) || isAdmin || havePermissionManagerAddPermission))
                return new SetRoleUserResponse();

            _repository.SetGlobalQueryFilterStatus(false);
            var role = await _repository.GetRoleByIdAsync(command.Request.RoleId, "Users");
            if (role == null)
                throw _exceptionService.RecordNotFoundException();


            var removedRoles = role.Users.Where(f => !command.Request.UserIds.Contains(f.UserId)).ToList();
            foreach (var item in removedRoles)
            {
                role.Users.Remove(item);
            }

            if (command.Request.UserIds != null && command.Request.UserIds.Any())
            {
                var alreadyExistUsers = role.Users.Where(c => command.Request.UserIds.Contains(c.UserId)).ToList();
                // var alreadyExistRoles = _roleRepository.SetGlobalQueryFilterStatus(false);
                foreach (var user in alreadyExistUsers)
                {
                    user.Status = RecordStatusEnum.Active;
                }

                var addedRoles = command.Request.UserIds
                    .Where(id =>
                        !alreadyExistUsers.Select(e => e.UserId).Contains(id) && role.Users.All(f => f.UserId != id))
                    .Select(c => new UserRole() { UserId = c, DateCreated = DateTime.Now }).ToList();
                foreach (var item in addedRoles)
                {
                    role.Users.Add(item);
                }
            }

            await _repository.UpdateAsync(role).ConfigureAwait(false);
            _repository.SetGlobalQueryFilterStatus(true);

            return new SetRoleUserResponse();
        }
    }
}
