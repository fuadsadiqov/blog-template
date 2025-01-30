using AutoMapper;
using GP.Core.Extensions;
using GP.Core.Models;
using GP.DataAccess;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Queries;
using GP.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GP.Application.Queries.UserQueries.GetAllUser
{
    public class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, GetAllUserResponse>
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AccessLimitService _accessLimitService;

        public GetAllUserQueryHandler(AuthService authService, IUserRepository userRepository, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, AccessLimitService accessLimitService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _accessLimitService = accessLimitService;
        }

        public async Task<GetAllUserResponse> Handle(GetAllUserQuery query, CancellationToken cancellationToken)
        {
            var acceptLanguage = _httpContextAccessor.HttpContext.GetAcceptLanguage();
            Expression<Func<User, bool>> filterPredicate = null;

            var filterParameters = query.Request.FilterParameters;
            if (filterParameters != null)
            {
                var isTextExist = !string.IsNullOrEmpty(filterParameters.Text);
                var isStatusExist = filterParameters.Status != null;
                var isIdsExist = filterParameters.Ids != null && filterParameters.Ids.Any();
                var isRoleIdsExist = filterParameters.RoleIds != null && filterParameters.RoleIds.Any();

                filterPredicate = c =>
                    (!isTextExist || (c.UserName.ToLower().Contains(filterParameters.Text.ToLowerInvariant()) ||
                                      c.Email.ToLower().Contains(filterParameters.Text.ToLowerInvariant()))) &&
                    (!isIdsExist || filterParameters.Ids.Contains(c.Id)) &&
                    (!isStatusExist || c.Status == filterParameters.Status) &&
                    (!isRoleIdsExist || c.Roles.Any(c => filterParameters.RoleIds.Contains(c.RoleId)));
            }

            //get total data count before paging
            var dataCount = await _userRepository.FindBy(filterPredicate)
                .CountAsync(cancellationToken: cancellationToken);

            var data = _userRepository.FindBy(filterPredicate);

            query.Request.SortParameters.NormalizeLocalizeSort<User>(acceptLanguage);
            //sorting
            data = data.SortBy(query.Request.SortParameters);

            //paging
            if (!query.Request.PagingParameters.IsAll)
                data = data.FindPaged(query.Request.PagingParameters);

            var list = await data.ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            list = list.Localize(acceptLanguage);

            var result = new List<UserInfoResponse>();
            foreach (var user in list)
            {
                var checkAndHideUserDetailAsync = await _accessLimitService.CheckAndHideUserDetailAsync(user);
                var userInfoResponse = _mapper.Map<User, UserInfoResponse>(user);
                if (!checkAndHideUserDetailAsync)
                {
                    userInfoResponse.Email = string
                        .Empty;
                }
                result.Add(userInfoResponse);
            }
            return new GetAllUserResponse()
            {
                Response = new FilteredDataResult<UserInfoResponse>()
                {
                    Items = result,
                    TotalCount = dataCount
                }
            };
        }
    }
}
