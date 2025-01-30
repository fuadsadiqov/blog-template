using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository;
using MediatR;
using GP.Core.Exceptions;
using GP.DataAccess.Repository.BlogRepository;
using GP.Application.Commands.BlogCommands.DeleteBlogTag;

namespace GP.Application.Commands.BlogCommands.DeleteBlog
{
    public class DeleteBlogCommandHandler : ICommandHandler<DeleteBlogCommand, DeleteBlogResponse>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;


        public DeleteBlogCommandHandler(IBlogRepository blogRepository,
            IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _blogRepository = blogRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<DeleteBlogResponse> Handle(DeleteBlogCommand command, CancellationToken cancellationToken)
        {
            var blogId = command.Request.Id;

            var blog = await _blogRepository.GetFirstAsync(b => b.Id == blogId);
            
            if(blog is null)
            {
                throw new RecordNotFoundException();
            }

            _blogRepository.Delete(blog);
            await _unitOfWork.CompleteAsync();

            await _mediator.Send(new DeleteBlogTagCommand(command.Request));

            return new DeleteBlogResponse
            {
            };
        }
    }
}







