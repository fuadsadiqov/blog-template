using AutoWrapper.Wrappers;
using GP.Core.Extensions;
using GP.Core.Resources;
using GP.DataAccess.Repository.UserJwt;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace GP.Application.Commands.AccountCommands.RefreshToken
{
    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IUserJwtRepository _jwtRepository;
        private readonly TokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RefreshTokenCommandHandler(IUserJwtRepository jwtRepository, TokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {

            _jwtRepository = jwtRepository;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext.GetRefreshToken();
            var refreshToken = await _jwtRepository.GetFirstAsync(c => c.Token == token, "User", "Impersonator");

            if (refreshToken is not { IsActive: true })
            {
                throw new ApiException("Invalid token", StatusCodes.Status400BadRequest, errorCode: ResourceKey.InvalidToken);
            }

            var isRevoked = refreshToken.IsRevoked;
            if (isRevoked)
            {
                await _tokenService.RevokeRefreshTokensAsync(token);
            }
            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = await _tokenService.RotateRefreshTokenAsync(refreshToken.Token);

            var newJwtToken = _tokenService.GenerateToken(refreshToken.User, refreshToken.ToTokenClaim().ToList().ToArray());

            // remove old refresh tokens from user
            await _tokenService.RemoveOldRefreshTokensAsync(refreshToken.UserId, refreshToken.Domain, refreshToken.Application, refreshToken.ImpersonatorId, true, c => c.Token != newRefreshToken.Token);

            return new RefreshTokenResponse()
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}