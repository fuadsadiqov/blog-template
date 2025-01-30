using AutoMapper;
using GP.Core.Extensions;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Queries;
using GP.Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace GP.Application.Queries.UserQueries.GetUser
{
    public class GetUserQueryHandler : IQueryHandler<GetUserQuery, GetUserResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ExceptionService _exceptionService;

        public GetUserQueryHandler(IMapper mapper, IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            ExceptionService exceptionService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _exceptionService = exceptionService;
        }

        public async Task<GetUserResponse> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            var acceptLanguage = _httpContextAccessor.HttpContext.GetAcceptLanguage();

            var item = await _userRepository.GetUserByIdAsync(query.Request.Id, "Roles.Role")
                .ConfigureAwait(false);

            if (item == null)
                throw _exceptionService.RecordNotFoundException();

            var result = _mapper.Map<User, UserResponse>(item);

            return new GetUserResponse
            {
                Response = result
            };
        }
    }
}