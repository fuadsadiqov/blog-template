using GP.Core.Resources;
using GP.DataAccess.Repository.UserRepository;
using GP.DataAccess.Repository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;

namespace GP.Application.Commands.UserCommands.UnLockUser
{
    public class UnLockUserCommandHandler : ICommandHandler<UnLockUserCommand, UnLockUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ExceptionService _exceptionService;

        public UnLockUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ExceptionService exceptionService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _exceptionService = exceptionService;
        }

        public async Task<UnLockUserResponse> Handle(UnLockUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(command.Request.UserId)
                .ConfigureAwait(false);

            if (user == null)
                throw _exceptionService.RecordNotFoundException(ResourceKey.UserNotFoundContactToAdmin);


            await _userRepository.SetUnLockoutAsync(user);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return new UnLockUserResponse();
        }
    }
}
