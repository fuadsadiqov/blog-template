using GP.Core.Enums;
using GP.Core.Extensions;
using GP.DataAccess.Repository.ConfirmationRepository;
using GP.DataAccess.Repository.UserRepository;
using GP.DataAccess.Repository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace GP.Application.Commands.UserCommands.StartPasswordRestoration
{
    public class StartPasswordRestorationCommandHandler : ICommandHandler<StartPasswordRestorationCommand, StartPasswordRestorationResponse>
    {
        private readonly IConfirmationRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;

        public StartPasswordRestorationCommandHandler(IConfirmationRepository repository, IUserRepository userRepository, UserManager<User> userManager, ExceptionService exceptionService, IUnitOfWork unitOfWork, EmailService emailService)
        {
            _repository = repository;
            _userRepository = userRepository;
            _userManager = userManager;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<StartPasswordRestorationResponse> Handle(StartPasswordRestorationCommand command, CancellationToken cancellationToken)
        {
            var emailOrUsername = command.Request.EmailOrUsername;
            var isEmail = emailOrUsername.IsEmail();

            User user;
            if (isEmail)
                user = await _userRepository.GetUserByEmailAsync(emailOrUsername)
                    .ConfigureAwait(false);
            else
                user = await _userRepository.GetUserByNameAsync(emailOrUsername)
                    .ConfigureAwait(false);

            if (user == null)
            {
                throw _exceptionService.RecordNotFoundException();
            }

            var email = user.Email;
            var confirmationHash = await _userRepository.GeneratePasswordResetTokenAsync(user);
            await _repository.AddAsync(new Confirmation()
            {
                ConfirmationHash = confirmationHash,
                Type = ConfirmationTypeEnum.Recovery,
                UserId = user.Id,
                Expiration = DateTime.Now.AddMinutes(3)
            });

            await _unitOfWork.CompleteAsync(cancellationToken);

            try
            {
                await _emailService.PasswordChangeEmail(email, confirmationHash);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return new StartPasswordRestorationResponse();
        }
    }
}
