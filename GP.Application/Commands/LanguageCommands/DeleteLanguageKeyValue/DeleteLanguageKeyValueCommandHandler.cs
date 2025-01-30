using GP.DataAccess.Repository.LanguageRepository;
using GP.DataAccess.Repository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;

namespace GP.Application.Commands.LanguageCommands.DeleteLanguageKeyValue
{
    public class DeleteLanguageKeyValueCommandHandler : ICommandHandler<DeleteLanguageKeyValueCommand,
        DeleteLanguageKeyValueResponse>
    {
        private readonly ILanguageKeyValueRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TranslationService _translationService;

        public DeleteLanguageKeyValueCommandHandler(ILanguageKeyValueRepository repository, IUnitOfWork unitOfWork,
            TranslationService translationService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _translationService = translationService;
        }

        public async Task<DeleteLanguageKeyValueResponse> Handle(DeleteLanguageKeyValueCommand command,
            CancellationToken cancellationToken)
        {
            var key = command.Request.Key;
            if (!string.IsNullOrEmpty(key))
            {
                _repository.DeleteWhere(c => c.Key == key, false);
                await _unitOfWork.CompleteAsync(cancellationToken);
                await _translationService.SetCacheAsync();
            }

            return new DeleteLanguageKeyValueResponse();
        }
    }
}
