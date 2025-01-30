using GP.Core.Constants;
using GP.Core.Enums.Enitity;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;

namespace GP.Application.Commands.UserCommands.SetUserPermission
{
    public class SetUserPermissionCommandHandler : ICommandHandler<SetUserPermissionCommand, SetUserPermissionResponse>
    {
        private readonly IUserRepository _repository;
        private readonly AuthService _authService;
        private readonly ExceptionService _exceptionService;

        public SetUserPermissionCommandHandler(IUserRepository repository, AuthService authService, ExceptionService exceptionService)
        {
            _repository = repository;
            _authService = authService;
            _exceptionService = exceptionService;
        }

        public async Task<SetUserPermissionResponse> Handle(SetUserPermissionCommand command, CancellationToken cancellationToken)
        {
            var isAdmin = await _authService.IsAdminAsync();
            if (!isAdmin)
                command.Request.PermissionIds = new List<string>();

            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            _repository.SetGlobalQueryFilterStatus(false);
            var user = await _repository.GetUserByIdAsync(command.Request.UserId, includeParams.ToArray());
            if (user == null)
                throw _exceptionService.RecordNotFoundException();

            var removedPermissions = user.DirectivePermissions.Where(f => !command.Request.PermissionIds.Contains(f.PermissionId)).ToList();
            foreach (var item in removedPermissions)
            {
                user.DirectivePermissions.Remove(item);
            }

            if (command.Request.PermissionIds != null && command.Request.PermissionIds.Any())
            {
                var alreadyExistPermissions = user.DirectivePermissions.Where(c => command.Request.PermissionIds.Contains(c.PermissionId)).ToList();
                foreach (var permission in alreadyExistPermissions)
                {
                    permission.Status = RecordStatusEnum.Active;
                }
                var addedPermissions = command.Request.PermissionIds.Where(id => !alreadyExistPermissions.Select(e => e.PermissionId).Contains(id) && user.DirectivePermissions.All(f => f.PermissionId != id)).Select(c => new UserPermission() { PermissionId = c, DateCreated = DateTime.Now }).ToList();
                foreach (var item in addedPermissions)
                {
                    user.DirectivePermissions.Add(item);
                }
            }

            return new SetUserPermissionResponse();
        }
    }
}
