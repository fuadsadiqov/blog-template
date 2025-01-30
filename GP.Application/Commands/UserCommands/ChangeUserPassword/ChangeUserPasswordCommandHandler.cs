using AutoMapper;
using AutoWrapper.Wrappers;
using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GP.Application.Commands.UserCommands.ChangeUserPassword
{
    public class ChangeUserPasswordCommandHandler : ICommandHandler<ChangeUserPasswordCommand, ChangeUserPasswordResponse>
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly ExceptionService _exceptionService;

        public ChangeUserPasswordCommandHandler(AuthService authService, IUserRepository userRepository, ExceptionService exceptionService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _exceptionService = exceptionService;
        }

        public async Task<ChangeUserPasswordResponse> Handle(ChangeUserPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(command.RequestedUserId).ConfigureAwait(false);
            if (user == null)
                throw _exceptionService.RecordNotFoundException();

            var authUserId = _authService.GetAuthorizedUserId();
            var isAdmin = false;
            if (!string.IsNullOrEmpty(authUserId))
                isAdmin = await _authService.IsAdminAsync(authUserId);

            IdentityResult result;
            if (command.ChangeWithoutOldPassword || isAdmin)
            {
                var token = await _userRepository.GeneratePasswordResetTokenAsync(user);
                result = await _userRepository.ResetPasswordAsync(user, token, command.Request.Password);
            }
            else
            {
                result =
                    await _userRepository.ChangePasswordAsync(user, command.Request.OldPassword, command.Request.Password).ConfigureAwait(false);
            }

            if (result.Succeeded) return new ChangeUserPasswordResponse();
            var errorMessage = result.Errors.FirstOrDefault();

            throw new ApiException(errorMessage?.Description, StatusCodes.Status409Conflict);
        }
    }
}
