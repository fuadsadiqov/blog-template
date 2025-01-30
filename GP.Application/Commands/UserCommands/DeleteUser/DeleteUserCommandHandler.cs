using GP.DataAccess.Repository.UserRepository;
using GP.DataAccess.Repository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;

namespace GP.Application.Commands.UserCommands.DeleteUser
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, DeleteUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthService _authService;

        public DeleteUserCommandHandler(IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork, AuthService authService)
        {
            _userRepository = userRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task<DeleteUserResponse> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(command.Request.UserId).ConfigureAwait(false);
            var isAdmin = await _authService.IsAdminAsync();

            var canEditRecord = isAdmin;
            if (!canEditRecord)
                throw _exceptionService.RecordNotEditableException();

            if (user == null)
                throw _exceptionService.RecordNotFoundException();

            await _userRepository.Delete(user);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return new DeleteUserResponse();
        }
    }
}
