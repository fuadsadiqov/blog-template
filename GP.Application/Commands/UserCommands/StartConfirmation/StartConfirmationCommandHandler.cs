using GP.DataAccess.Repository.UserRepository;
using GP.DataAccess.Repository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository.ConfirmationRepository;
using GP.Domain.Entities.Identity;
using GP.Core.Enums;

namespace GP.Application.Commands.UserCommands.StartConfirmation
{
    public class StartConfirmationCommandHandler : ICommandHandler<StartConfirmationCommand, StartConfirmationResponse>
    {
        private readonly IConfirmationRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;

        public StartConfirmationCommandHandler(IConfirmationRepository repository, IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork, EmailService emailService)
        {
            _repository = repository;
            _userRepository = userRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<StartConfirmationResponse> Handle(StartConfirmationCommand command, CancellationToken cancellationToken)
        {
            var userId = command.Request.UserId;
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw _exceptionService.UserNotFoundException();
            }

            var confirmationHash = await _userRepository.GeneratePasswordResetTokenAsync(user);
            await _repository.AddAsync(new Confirmation()
            {
                ConfirmationHash = confirmationHash,
                Type = ConfirmationTypeEnum.Registration,
                UserId = userId,
                Expiration = DateTime.Now.AddDays(1)
            });

            await _unitOfWork.CompleteAsync(cancellationToken);

            var userRoles = user.Roles;

            if (!(userRoles.Count == 1 && userRoles.Any(x => x.Role.Name == "ASSIGNEE")))
            {
                try
                {
                    await _emailService.UserRegistrationEmail(user.Email!, confirmationHash);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return new StartConfirmationResponse();
        }
    }
}
