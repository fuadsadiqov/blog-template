using AutoMapper;
using GP.Application.Commands.RoleCommands.SetRolePermission;
using GP.Application.Commands.RoleCommands.SetRoleUser;
using GP.Core.Constants;
using GP.DataAccess.Repository;
using GP.DataAccess.Repository.RoleRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using MediatR;

namespace GP.Application.Commands.RoleCommands.UpdateRole
{
    public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand, UpdateRoleResponse>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRoleCommandHandler(IRoleRepository repository, IMapper mapper, IMediator mediator,
            ExceptionService exceptionService, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateRoleResponse> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            var role = await _repository
                .GetRoleByIdAsync(command.Request.Id, new IncludeStringConstants().RolePermissionIncludeList.ToArray())
                .ConfigureAwait(false);
            if (role == null)
                throw _exceptionService.RecordNotFoundException();

            var isExistAsync =
                await _repository.IsExistAsync(c => c.Name == command.Request.Name).ConfigureAwait(false) &&
                command.Request.Name != role.Name;
            if (isExistAsync)
                throw _exceptionService.RecordAlreadyExistException();

            role.Name = command.Request.Name;
            role.Description = command.Request.Description;

            //update
            await _repository.UpdateAsync(role).ConfigureAwait(false);
            await _mediator.Send(new SetRoleUserCommand(new SetRoleUserRequest()
            {
                RoleId = role.Id,
                UserIds = command.Request.UserIds
            }), cancellationToken);

            await _mediator.Send(new SetRolePermissionCommand(new SetRolePermissionRequest()
            {
                RoleId = role.Id,
                PermissionsIds = command.Request.PermissionIds
            }), cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken).ConfigureAwait(false);

            return new UpdateRoleResponse()
            {
                Response = role.Id
            };
        }
    }
}
