using AutoMapper;
using GP.Application.Commands.UserCommands.SetUserPermission;
using GP.Application.Commands.UserCommands.SetUserRole;
using GP.Core.Constants;
using GP.Core.Enums.Enitity;
using GP.DataAccess.Repository;
using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using MediatR;

namespace GP.Application.Commands.UserCommands.UpdateUser
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UpdateUserResponse>
    {
        private readonly AuthService _authService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(AuthService authService, IMapper mapper, IUserRepository userRepository, IMediator mediator, ExceptionService exceptionService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _mapper = mapper;
            _userRepository = userRepository;
            _mediator = mediator;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateUserResponse> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var userId = _authService.GetAuthorizedUserId();
            var havePermissionManagerAddPermission =
                await _authService.UserIsInPermissionAsync(userId, $"permissionmanager_{PermissionEnum.Add}");
            var haveRoleSetPermission = await _authService.UserIsInPermissionAsync(userId, $"role_{PermissionEnum.Set}");
            var haveRoleListPermission = await _authService.UserIsInPermissionAsync(userId, $"role_{PermissionEnum.List}");
            var isAdmin = await _authService.IsAdminAsync();

            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            var user = await _userRepository.GetUserByIdAsync(command.Request.Id, includeParams.ToArray()).ConfigureAwait(false);
            if (user == null)
                throw _exceptionService.RecordNotFoundException();

            //update
            _mapper.Map(command.Request, user);

            if (((haveRoleSetPermission && haveRoleListPermission) || isAdmin || havePermissionManagerAddPermission))
            {
                await _mediator.Send(new SetUserRoleCommand(new SetUserRoleRequest()
                {
                    UserId = user.Id,
                    RoleIds = command.Request.Roles
                }), cancellationToken);

                await _mediator.Send(new SetUserPermissionCommand(new SetUserPermissionRequest()
                {
                    PermissionIds = command.Request.DirectivePermissions,
                    UserId = user.Id
                }), cancellationToken);
            }

            await _userRepository.UpdateAsync(user).ConfigureAwait(false);
            await _unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);

            return new UpdateUserResponse()
            {
                Response = user
            };
        }
    }
}
