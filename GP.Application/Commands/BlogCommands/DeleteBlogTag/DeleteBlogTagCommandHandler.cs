using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository;
using MediatR;
using GP.DataAccess.Repository.BlogTagRepository;
using Microsoft.EntityFrameworkCore;

namespace GP.Application.Commands.BlogCommands.DeleteBlogTag
{
    public class DeleteBlogTagCommandHandler : ICommandHandler<DeleteBlogTagCommand, DeleteBlogTagResponse>
    {
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;


        public DeleteBlogTagCommandHandler(IBlogTagRepository blogTagRepository,
            IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _blogTagRepository = blogTagRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<DeleteBlogTagResponse> Handle(DeleteBlogTagCommand command, CancellationToken cancellationToken)
        {
            var blogId = command.Request.Id;

            var blogTags = await _blogTagRepository.FindBy(bT => bT.BlogId == blogId).ToListAsync(cancellationToken);

            foreach (var blogTag in blogTags)
            {
                _blogTagRepository.Delete(blogTag, false);
            }

            await _unitOfWork.CompleteAsync();

            return new DeleteBlogTagResponse
            {
            };
        }
    }
}







