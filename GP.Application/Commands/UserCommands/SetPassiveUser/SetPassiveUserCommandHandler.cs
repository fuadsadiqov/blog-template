using GP.Core.Enums.Enitity;
using GP.DataAccess.Repository.UserRepository;
using GP.DataAccess.Repository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;

namespace GP.Application.Commands.UserCommands.SetPassiveUser
{
    public class SetPassiveUserCommandHandler : ICommandHandler<SetPassiveUserCommand, SetPassiveUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthService _authService;

        public SetPassiveUserCommandHandler(IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork, AuthService authService)
        {
            _userRepository = userRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task<SetPassiveUserResponse> Handle(SetPassiveUserCommand command, CancellationToken cancellationToken)
        {
            var isAdmin = await _authService.IsAdminAsync();
            if (!isAdmin)
            {
                throw _exceptionService.UnauthorizedException();
            }

            var user = await _userRepository
                .GetUserByIdAsync(command.Request.Id)
                .ConfigureAwait(false);

            if (user == null)
                throw _exceptionService.RecordNotFoundException();

            user.DateModified = DateTime.Now;
            user.Status = command.Request.IsPassive ? RecordStatusEnum.Passive : RecordStatusEnum.Active;

            await _unitOfWork.CompleteAsync(cancellationToken);
            return new SetPassiveUserResponse();
        }
    }
}
