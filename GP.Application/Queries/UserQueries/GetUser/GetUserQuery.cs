﻿using GP.Infrastructure.Configurations.Queries;

namespace GP.Application.Queries.UserQueries.GetUser
{
    public class GetUserQuery : IQuery<GetUserResponse>
    {
        public GetUserQuery(GetUserRequest request)
        {
            Request = request;
        }

        public GetUserRequest Request { get; set; }
    }
}
