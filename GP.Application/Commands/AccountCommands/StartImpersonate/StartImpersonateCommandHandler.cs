using AutoWrapper.Wrappers;
using GP.Application.Queries.UserQueries.GetAuthUser;
using GP.Core.Models;
using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using MediatR;
using System.Security.Claims;

namespace GP.Application.Commands.AccountCommands.StartImpersonate
{
    public class StartImpersonateCommandHandler : ICommandHandler<StartImpersonateCommand, StartImpersonateResponse>
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly AuthService _authService;
        private readonly TokenService _tokenService;

        public StartImpersonateCommandHandler(IMediator mediator, IUserRepository userRepository, AuthService authService, TokenService tokenService)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _authService = authService;
            _tokenService = tokenService;
        }

        public async Task<StartImpersonateResponse> Handle(StartImpersonateCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(command.Request.UserId);

            var authUser = (await _mediator.Send(new GetAuthUserQuery(new GetAuthUserRequest()), cancellationToken)).Response;

            if (user.Id == authUser.Id)
                throw new ApiException("Can't impersonate yourself!");

            var tokenClaim = _authService.GetTokenClaim();
            var impersonatorIdClaim = new Claim(CustomClaimTypes.Impersonator, authUser.Id);
            var impersonatorNameClaim = new Claim(CustomClaimTypes.ImpersonatorName, authUser.UserName);
            tokenClaim.ImpersonatorId = impersonatorIdClaim;
            tokenClaim.ImpersonatorName = impersonatorNameClaim;
            var claims = tokenClaim.ToList();

            var accessToken = _tokenService.GenerateToken(user, claims.ToArray());
            await _tokenService.RemoveOldRefreshTokensAsync(user.Id, tokenClaim.Domain?.Value, tokenClaim.Application?.Value, impersonatorIdClaim.Value, removeAll: false);
            var refreshToken = await _tokenService.AddRefreshTokenAsync(user.Id, tokenClaim);

            return new StartImpersonateResponse()
            {
                Token = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
