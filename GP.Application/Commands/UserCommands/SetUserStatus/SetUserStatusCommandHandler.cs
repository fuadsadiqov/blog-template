using GP.Core.Enums.Enitity;
using GP.Core.Resources;
using GP.DataAccess.Repository.UserRepository;
using GP.DataAccess.Repository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;

namespace GP.Application.Commands.UserCommands.SetUserStatus
{
    public class SetUserStatusCommandHandler : ICommandHandler<SetUserStatusCommand, SetUserStatusResponse>
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ExceptionService _exceptionService;
        private readonly TokenService _tokenService;

        public SetUserStatusCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork, ExceptionService exceptionService, TokenService tokenService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _exceptionService = exceptionService;
            _tokenService = tokenService;
        }

        public async Task<SetUserStatusResponse> Handle(SetUserStatusCommand command, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByIdAsync(command.Request.UserId)
                .ConfigureAwait(false);

            if (user == null)
                throw _exceptionService.RecordNotFoundException(ResourceKey.UserNotFoundContactToAdmin);

            if (command.Request.Status is (RecordStatusEnum.Deleted or RecordStatusEnum.Passive))
            {
                await _tokenService.RemoveOldRefreshTokensAsync(user.Id, string.Empty, string.Empty, removeAll: true);
            }

            user.Status = command.Request.Status;

            await _unitOfWork.CompleteAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

            return new SetUserStatusResponse();
        }
    }
}
