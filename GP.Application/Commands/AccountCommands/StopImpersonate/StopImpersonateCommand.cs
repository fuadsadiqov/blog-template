﻿using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.AccountCommands.StopImpersonate
{
    public class StopImpersonateCommand : CommandBase<StopImpersonateResponse>
    {
        public StopImpersonateCommand(StopImpersonateRequest request)
        {
            Request = request;
        }

        public StopImpersonateRequest Request { get; set; }
    }
}
