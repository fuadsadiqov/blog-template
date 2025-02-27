using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using GP.Core.Extensions;
using GP.Core.Models;
using GP.DataAccess.Repository.UserRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.Application.Commands.UserCommands.CreateUserWithGoogle;
using GP.Core.Enums.Enitity;

namespace GP.Application.Commands.AccountCommands.SignInUserWithGoogle
{
    public class SignInUserWithGoogleCommandHandler : ICommandHandler<SignInUserWithGoogleCommand, SignInUserWithGoogleResponse>
    {
        private readonly SignInService _signInService;
        private readonly IUserRepository _userRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IMediator _mediator;
        
        public SignInUserWithGoogleCommandHandler(SignInService signInService, IUserRepository userRepository, ExceptionService exceptionService, IMediator mediator)
        {
            _signInService = signInService;
            _userRepository = userRepository;
            _exceptionService = exceptionService;
            _mediator = mediator;
        }
        
        public async Task<SignInUserWithGoogleResponse> Handle(SignInUserWithGoogleCommand command, CancellationToken cancellationToken)
        {
            var email = command.Request.Email;
            var fullName = command.Request.FullName;

            User user;
                user = await _userRepository.GetUserByEmailAsync(email)
                    .ConfigureAwait(false);
           
            if (user == null)
            {
                
                var newUser = (await RegisterUser(fullName, email));
                if (newUser.Response == null)
                    throw _exceptionService.InvalidSignInException();
                user = await _userRepository.GetUserByEmailAsync(email)
                    .ConfigureAwait(false);
            }
                
            var isActiveUser = user.Status == RecordStatusEnum.Active;
            if (!isActiveUser)
                throw _exceptionService.AccessDeniedException();

            var isSignableUser = (await SignInUser(user)).Succeeded;
            if (!isSignableUser)
                throw _exceptionService.InvalidSignInException();

            return new SignInUserWithGoogleResponse()
            {
                IsSigned = isSignableUser
            };
        }
        private async Task<UserSignInResult> SignInUser(User user)
        {
            UserSignInResult signInResult;

            signInResult = await _signInService.SignInWithGoogleAsync(user);

            if (signInResult.IsLockOut)
                throw _exceptionService.LockedUserException();

            return signInResult;
        }

        private async Task<CreateUserWithGoogleResponse> RegisterUser(string fullName, string email)
        {
            CreateUserWithGoogleRequest request = new() { Email = email, FullNameAz = fullName };
            var result = await _mediator.Send(new CreateUserWithGoogleCommand(request));
            
            if (result == null)
                throw _exceptionService.LockedUserException();
            
            return result;
        }
    }
}







