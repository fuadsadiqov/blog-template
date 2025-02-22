﻿using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.AccountCommands.StartImpersonate
{
    public class StartImpersonateCommand : CommandBase<StartImpersonateResponse>
    {
        public StartImpersonateCommand(StartImpersonateRequest request)
        {
            Request = request;
        }

        public StartImpersonateRequest Request { get; set; }
    }
}
