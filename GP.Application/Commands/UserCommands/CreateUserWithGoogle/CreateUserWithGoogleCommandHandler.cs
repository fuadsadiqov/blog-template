using AutoMapper;
using AutoWrapper.Wrappers;
using GP.Application.Commands.UserCommands.SetUserPermission;
using GP.Application.Commands.UserCommands.SetUserRole;
using GP.DataAccess.Repository;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GP.Application.Commands.UserCommands.CreateUserWithGoogle
{
    public class CreateUserWithGoogleCommandHandler : ICommandHandler<CreateUserWithGoogleCommand, CreateUserWithGoogleResponse>
    {
        private readonly AuthService _authService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;
        
        public CreateUserWithGoogleCommandHandler(AuthService authService, IMapper mapper, IUserRepository userRepository, IMediator mediator, ExceptionService exceptionService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _mapper = mapper;
            _userRepository = userRepository;
            _mediator = mediator;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateUserWithGoogleResponse> Handle(CreateUserWithGoogleCommand command, CancellationToken cancellationToken)
        {
            var isExistAsync = await _userRepository
                .IsExistAsync(c => c.Email.ToLower() == command.Request.Email.ToLower())
                .ConfigureAwait(false);
            command.Request.FullNameEn = command.Request.FullNameAz;
            if (isExistAsync)
                throw _exceptionService.RecordAlreadyExistException();

            var user = _mapper.Map<CreateUserWithGoogleRequest, User>(command.Request);
            user.Id = Guid.NewGuid().ToString();
            user.EmailConfirmed = true;
            user.UserName = command.Request.Email;
            var result = await _userRepository.CreateWithoutPasswordAsync(user).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                var errorMessage = result.Errors.FirstOrDefault();
                throw new ApiException(errorMessage, StatusCodes.Status409Conflict);
            }

            if (result.Succeeded)
            {
                await _unitOfWork.CompleteAsync(cancellationToken);
                
                // await _mediator.Send(new SetUserRoleCommand(new SetUserRoleRequest()
                // {
                //     RoleIds = command.Request.Roles,
                //     UserId = user.Id
                // }), cancellationToken);
                //
                // await _mediator.Send(new SetUserPermissionCommand(new SetUserPermissionRequest()
                // {
                //     PermissionIds = command.Request.DirectivePermissions,
                //     UserId = user.Id
                // }), cancellationToken);
            }

            return new CreateUserWithGoogleResponse()
            {
                Response = user
            };
        }
    }
}
