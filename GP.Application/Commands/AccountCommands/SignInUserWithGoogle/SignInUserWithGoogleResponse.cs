﻿namespace GP.Application.Commands.AccountCommands.SignInUserWithGoogle
{
    public class SignInUserWithGoogleResponse
    {
        public bool IsSigned { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool RequiredConfirm { get; set; }
        public bool IsPinRequired { get; set; }
        public string Message { get; set; }

    }
}
