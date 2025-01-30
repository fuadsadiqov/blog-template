using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository;
using GP.Core.Exceptions;
using GP.DataAccess.Repository.TagRepository;

namespace GP.Application.Commands.TagCommands.DeleteTagCommands
{
    public class DeleteTagCommandHandler : ICommandHandler<DeleteTagCommand, DeleteTagResponse>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;


        public DeleteTagCommandHandler(ITagRepository tagRepository,
            IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork)
        {
            _tagRepository = tagRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteTagResponse> Handle(DeleteTagCommand command, CancellationToken cancellationToken)
        {
            var id = command.Request.Id;

            var tag = await _tagRepository.GetFirstAsync(c => c.Id == id);
            if(tag is null)
            {
                throw new RecordNotFoundException("Tag not found");
            }
            _tagRepository.Delete(tag);

            await _unitOfWork.CompleteAsync();

            return new DeleteTagResponse
            {
            };
        }
    }
}







