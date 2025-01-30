using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using MediatR;

namespace GP.Application.Commands.AccountCommands.StopImpersonate
{
    public class StopImpersonateCommandHandler : ICommandHandler<StopImpersonateCommand, StopImpersonateResponse>
    {
        private readonly IMediator _mediator;
        private readonly AuthService _authService;
        private readonly TokenService _tokenService;

        public StopImpersonateCommandHandler(IMediator mediator, AuthService authService, TokenService tokenService)
        {
            _mediator = mediator;
            _authService = authService;
            _tokenService = tokenService;
        }

        public async Task<StopImpersonateResponse> Handle(StopImpersonateCommand command, CancellationToken cancellationToken)
        {
            var authUserId = _authService.GetAuthorizedUserId();
            var tokenClaim = _authService.GetTokenClaim();

            if (tokenClaim.ImpersonatorId != null)
            {
                var impersonatorIsAdmin = await _authService.IsAdminAsync(tokenClaim.ImpersonatorId.Value);
                if (impersonatorIsAdmin)
                {
                    await _tokenService.RemoveOldRefreshTokensAsync(authUserId, tokenClaim.Domain?.Value, tokenClaim.Application?.Value, tokenClaim.ImpersonatorId?.Value, removeAll: true);
                }

            }

            return new StopImpersonateResponse()
            {

            };
        }
    }
}
