using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository;
using GP.Core.Enums.Enitity;
using GP.Core.Exceptions;
using MediatR;
using GP.Domain.Entities.Common;
using GP.DataAccess.Repository.TagRepository;

namespace GP.Application.Commands.TagCommands.AddTag
{
    public class AddTagCommandHandler : ICommandHandler<AddTagCommand, AddTagResponse>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;


        public AddTagCommandHandler(ITagRepository tagRepository,
            IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _tagRepository = tagRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<AddTagResponse> Handle(AddTagCommand command, CancellationToken cancellationToken)
        {
            var name = command.Request.Name;
            var isExist = await _tagRepository.GetFirstAsync(t => t.Name == name);
            if (isExist == null)
            {
                var id = Guid.NewGuid();
                var tag = new Tag()
                {
                    Id = id,
                    Name = name,
                    Status = RecordStatusEnum.Active,
                    DateCreated = DateTime.Now,
                };
                await _tagRepository.AddAsync(tag);

                await _unitOfWork.CompleteAsync();

            return new AddTagResponse
            {
                Id = id
            };
            }
            else
            {
                throw new RecordAlreadyExistException("Tag already exists");
            }
        }
    }
}







