﻿using MediatR;

namespace GP.Infrastructure.Configurations.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {

    }
}
