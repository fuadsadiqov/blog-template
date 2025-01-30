using GP.DataAccess.Repository.UserJwt;
using GP.DataAccess.Repository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace GP.Application.Commands.AccountCommands.SignOutUser
{
    public class SignOutUserCommandHandler : ICommandHandler<SignOutUserCommand, SignOutUserResponse>
    {
        private readonly AuthService _authService;
        private readonly TokenService _tokenService;

        public SignOutUserCommandHandler(AuthService authService, TokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        public async Task<SignOutUserResponse> Handle(SignOutUserCommand request, CancellationToken cancellationToken)
        {
            var authUserId = _authService.GetAuthorizedUserId();
            var tokenClaim = _authService.GetTokenClaim();

            await _tokenService.RemoveOldRefreshTokensAsync(authUserId, tokenClaim.Domain?.Value, tokenClaim.Application?.Value,
                tokenClaim.ImpersonatorId?.Value, true);

            return new SignOutUserResponse();
        }
    }
}
