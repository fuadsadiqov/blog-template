using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository;
using GP.Core.Enums.Enitity;
using MediatR;
using GP.Application.Commands.BlogCommands.SetBlogTag;
using GP.DataAccess.Repository.BlogRepository;
using GP.Domain.Entities.Common;

namespace GP.Application.Commands.BlogCommands.AddBlog
{
    public class AddBlogCommandHandler : ICommandHandler<AddBlogCommand, AddBlogResponse>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;


        public AddBlogCommandHandler(IBlogRepository blogRepository,
            IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _blogRepository = blogRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<AddBlogResponse> Handle(AddBlogCommand command, CancellationToken cancellationToken)
        {
            var name = command.Request.Name;
            var description = command.Request.Description;
            var coverImage = command.Request.CoverImage;
            var categoryId = command.Request.CategoryId;

            var id = Guid.NewGuid();
            var blog = new Blog()
            {
                Id = id,
                Name = name,
                Description = description,
                CoverImage = coverImage,
                CategoryId = categoryId,
                Status = RecordStatusEnum.Active,
                DateCreated = DateTime.Now,
            };
            await _blogRepository.AddAsync(blog);

            await _unitOfWork.CompleteAsync();

            command.Request.Tags.BlogId = id;
            
            await _mediator.Send(new SetBlogTagCommand(command.Request.Tags));

            await _unitOfWork.CompleteAsync();

            return new AddBlogResponse
            {
            };
        }
    }
}







