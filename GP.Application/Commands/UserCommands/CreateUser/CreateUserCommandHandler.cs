﻿using AutoMapper;
using AutoWrapper.Wrappers;
using GP.Application.Commands.UserCommands.SetUserPermission;
using GP.Application.Commands.UserCommands.SetUserRole;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GP.Application.Commands.UserCommands.CreateUser
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly AuthService _authService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly ExceptionService _exceptionService;

        public CreateUserCommandHandler(AuthService authService, IMapper mapper, IUserRepository userRepository, IMediator mediator, ExceptionService exceptionService)
        {
            _authService = authService;
            _mapper = mapper;
            _userRepository = userRepository;
            _mediator = mediator;
            _exceptionService = exceptionService;
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var isExistAsync = await _userRepository
                .IsExistAsync(c => c.UserName.ToLower() == command.Request.UserName.ToLowerInvariant() || c.Email.ToLower() == command.Request.Email.ToLower())
                .ConfigureAwait(false);

            if (isExistAsync)
                throw _exceptionService.RecordAlreadyExistException();

            var user = _mapper.Map<CreateUserRequest, User>(command.Request);
            user.Id = Guid.NewGuid().ToString();
            user.EmailConfirmed = true;

            var result = await _userRepository.CreateAsync(user, command.Request.Password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                var errorMessage = result.Errors.FirstOrDefault();
                _exceptionService.WrongRequestException(errorMessage.Description);
            }

            if (result.Succeeded)
            {
                if (command.Request.Roles != null && command.Request.Roles.Any())
                {
                    await _mediator.Send(new SetUserRoleCommand(new SetUserRoleRequest()
                    {
                        RoleIds = command.Request.Roles,
                        UserId = user.Id
                    }), cancellationToken);
                }

                if (command.Request.Roles != null && command.Request.Roles.Any())
                {
                    await _mediator.Send(new SetUserPermissionCommand(new SetUserPermissionRequest()
                    {
                        PermissionIds = command.Request.DirectivePermissions,
                        UserId = user.Id
                    }), cancellationToken);
                }
            }

            return new CreateUserResponse()
            {
                Response = user
            };
        }
    }
}
