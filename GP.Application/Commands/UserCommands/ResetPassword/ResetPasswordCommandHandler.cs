using GP.DataAccess.Repository.ConfirmationRepository;
using GP.DataAccess.Repository.UserRepository;
using GP.DataAccess.Repository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;

namespace GP.Application.Commands.UserCommands.ResetPassword
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, ResetPasswordResponse>
    {
        private readonly IConfirmationRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordCommandHandler(IConfirmationRepository repository, IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _userRepository = userRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResetPasswordResponse> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var newPassword = command.Request.NewPassword;
            var confirmationHash = command.Request.ConfirmationHash;
            var confirmation = await _repository.GetFirstAsync(x => x.ConfirmationHash == confirmationHash, "User") ?? throw _exceptionService.RecordNotFoundException();
            var user = confirmation.User;

            await _userRepository.ResetPasswordAsync(user, confirmationHash, newPassword);

            _repository.Delete(confirmation);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return new ResetPasswordResponse();
        }
    }
}
