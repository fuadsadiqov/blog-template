using GP.Core.Resources;
using GP.DataAccess.Repository.UserRepository;
using GP.DataAccess.Repository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;

namespace GP.Application.Commands.UserCommands.LockUser
{
    public class LockUserCommandHandler : ICommandHandler<LockUserCommand, LockUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ExceptionService _exceptionService;

        public LockUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ExceptionService exceptionService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _exceptionService = exceptionService;
        }

        public async Task<LockUserResponse> Handle(LockUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(command.Request.UserId)
                .ConfigureAwait(false);

            if (user == null)
                throw _exceptionService.RecordNotFoundException(ResourceKey.UserNotFoundContactToAdmin);

            await _userRepository.SetLockoutEnabledAsync(user, command.Request.ExpireDate);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return new LockUserResponse();
        }
    }
}
