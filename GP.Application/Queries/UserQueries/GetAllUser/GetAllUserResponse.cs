using GP.Core.Models;

namespace GP.Application.Queries.UserQueries.GetAllUser
{
    public class GetAllUserResponse
    {
        public FilteredDataResult<UserInfoResponse> Response { get; set; }
    }
}
