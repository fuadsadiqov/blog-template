using GP.Application.Commands.BlogCommands.DeleteBlog;
using GP.Application.Commands.BlogCommands.DeleteBlogTag;
using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository;
using GP.Core.Enums.Enitity;
using MediatR;
using GP.Application.Commands.BlogCommands.SetBlogTag;
using GP.DataAccess.Repository.BlogRepository;
using GP.Domain.Entities.Common;

namespace GP.Application.Commands.BlogCommands.UpdateBlog
{
    public class UpdateBlogCommandHandler : ICommandHandler<UpdateBlogCommand, UpdateBlogResponse>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;


        public UpdateBlogCommandHandler(IBlogRepository blogRepository,
            IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _blogRepository = blogRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<UpdateBlogResponse> Handle(UpdateBlogCommand command, CancellationToken cancellationToken)
        {
            var id = command.Request.Id;
            var name = command.Request.Name;
            var description = command.Request.Description;
            var coverImage = command.Request.CoverImage;
            var categoryId = command.Request.CategoryId;
            
            var blog = await _blogRepository.GetFirstAsync(b => b.Id == id);
            blog.Name = name;
            blog.Description = description;
            blog.CoverImage = coverImage;
            blog.CategoryId = categoryId;
            blog.DateModified = DateTime.UtcNow;
            
            _blogRepository.Update(blog);

            await _unitOfWork.CompleteAsync();

            if (command.Request.Tags.Length > 0)
            {
                List<Guid> tagIds = command.Request.Tags
                    .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries) 
                    .Select(Guid.Parse) 
                    .ToList();

                SetBlogTagRequest blogTagRequest = new SetBlogTagRequest();
                blogTagRequest.TagIds = tagIds;
                blogTagRequest.BlogId = id;
                
                await _mediator.Send(new SetBlogTagCommand(blogTagRequest));

                await _unitOfWork.CompleteAsync();
   
            }

            return new UpdateBlogResponse
            {
            };
        }
    }
}







