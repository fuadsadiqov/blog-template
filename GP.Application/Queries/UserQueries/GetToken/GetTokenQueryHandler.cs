using GP.Core.Models;
using GP.DataAccess.Repository;
using GP.DataAccess.Repository.UserJwt;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Queries;
using GP.Infrastructure.Services;

namespace GP.Application.Queries.UserQueries.GetToken
{
    public class GetTokenQueryHandler : IQueryHandler<GetTokenQuery, GetTokenResponse>
    {
        private readonly TokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IUserJwtRepository _userJwtRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ExceptionService _exceptionService;

        public GetTokenQueryHandler(TokenService tokenService, IUserRepository userRepository, IUserJwtRepository userJwtRepository, IUnitOfWork unitOfWork, ExceptionService exceptionService)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _userJwtRepository = userJwtRepository;
            _unitOfWork = unitOfWork;
            _exceptionService = exceptionService;
        }

        public async Task<GetTokenResponse> Handle(GetTokenQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(query.Request.UserId);
            if (user == null)
            {
                throw _exceptionService.RecordNotFoundException();
            }

            var claims = query.Request.Claims;
            var applicationClaim = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Application);
            var rememberMeClaim = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.RememberMe);
            var rememberMe = rememberMeClaim?.Value.ToLower() == bool.TrueString.ToLower();
            var newToken = _tokenService.GenerateToken(user, claims.ToArray());

            await _userJwtRepository.AddAsync(new UserJwt()
            {
                Token = newToken,
                UserId = user.Id,
                Application = applicationClaim?.Value,
                RememberMe = rememberMeClaim?.Value,
                DateCreated = DateTime.Now
            });

            await _unitOfWork.CompleteAsync(cancellationToken);

            return new GetTokenResponse()
            {
                Token = newToken
            };
        }
    }
}
