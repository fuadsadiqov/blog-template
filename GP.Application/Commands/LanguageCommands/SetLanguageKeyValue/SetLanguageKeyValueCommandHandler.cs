using GP.DataAccess.Repository.LanguageRepository;
using GP.DataAccess.Repository;
using GP.Domain.Entities.Lang;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;

namespace GP.Application.Commands.LanguageCommands.SetLanguageKeyValue
{
    public class SetLanguageKeyValueCommandHandler : ICommandHandler<SetLanguageKeyValueCommand, SetLanguageKeyValueResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILanguageKeyValueRepository _repository;
        private readonly ILanguageRepository _languageRepository;
        private readonly ExceptionService _exceptionService;
        private readonly TranslationService _translationService;

        public SetLanguageKeyValueCommandHandler(IUnitOfWork unitOfWork, ILanguageKeyValueRepository repository,
            ExceptionService exceptionService, ILanguageRepository languageRepository, TranslationService translationService)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _exceptionService = exceptionService;
            _languageRepository = languageRepository;
            _translationService = translationService;
        }

        public async Task<SetLanguageKeyValueResponse> Handle(SetLanguageKeyValueCommand command,
            CancellationToken cancellationToken)
        {
            var languageCode = command.Request.LanguageCode;

            var key = command.Request.Key;
            var value = command.Request.Value;

            var langIsExist = await _languageRepository.IsExistAsync(c => c.Code == languageCode);
            if (!langIsExist)
                throw _exceptionService.RecordNotFoundException();

            var languageKeyValue = await _repository.GetFirstAsync(c =>
                c.LanguageCode == languageCode && c.Key == key);

            if (languageKeyValue is null)
            {
                languageKeyValue = new LanguageKeyValue()
                {
                    Value = value,
                    Key = key,
                    LanguageCode = languageCode
                };
                await _repository.AddAsync(languageKeyValue);
            }
            else languageKeyValue.Value = value;

            await _unitOfWork.CompleteAsync(cancellationToken);

            await _translationService.SetCacheAsync();

            return new SetLanguageKeyValueResponse(languageCode, key, value);
        }
    }
}
