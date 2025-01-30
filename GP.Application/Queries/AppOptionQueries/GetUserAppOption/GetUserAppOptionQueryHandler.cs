using AutoMapper;
using GP.DataAccess.Repository.AppOptionRepository;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.App;
using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.Queries.AppOptionQueries.GetUserAppOption
{
    public class GetUserAppOptionQueryHandler : IQueryHandler<GetUserAppOptionQuery, GetUserAppOptionResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IAppOptionRepository _appOptionRepository;

        public GetUserAppOptionQueryHandler(IMapper mapper, IUserRepository userRepository, IAppOptionRepository appOptionRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _appOptionRepository = appOptionRepository;
        }

        public async Task<GetUserAppOptionResponse> Handle(GetUserAppOptionQuery query,
            CancellationToken cancellationToken)
        {
            var data = await _appOptionRepository.GetFirstAsync(c => c.UserId == query.Request.UserId, "User");
            var result = _mapper.Map<AppOption, AppOptionResponse>(data);

            return new GetUserAppOptionResponse()
            {
                Response = result

            };
        }
    }
}
