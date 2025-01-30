using AutoMapper;
using GP.Application.Commands.RoleCommands.SetRolePermission;
using GP.Application.Commands.RoleCommands.SetRoleUser;
using GP.DataAccess.Repository.RoleRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using MediatR;

namespace GP.Application.Commands.RoleCommands.CreateRole
{
    public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, CreateRoleResponse>
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ExceptionService _exceptionService;

        public CreateRoleCommandHandler(IRoleRepository repository, IMapper mapper, IMediator mediator,
            ExceptionService exceptionService)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
            _exceptionService = exceptionService;
        }

        public async Task<CreateRoleResponse> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            var isExistAsync =
                await _repository.IsExistAsync(c => c.Name == command.Request.Name).ConfigureAwait(false);
            if (isExistAsync)
                throw _exceptionService.WrongRequestException(
                    $"Role with name '{command.Request.Name}' already exists.");

            var role = new Role
            {
                Id = Guid.NewGuid().ToString(),
                Name = command.Request.Name,
                Description = command.Request.Description
            };

            try
            {
                await _repository.CreateAsync(role).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw _exceptionService.RecordAlreadyExistException();
            }

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

            return new CreateRoleResponse()
            {
                Response = role.Id
            };
        }
    }
}
