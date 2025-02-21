using GP.Infrastructure.Configurations.Commands;
using GP.DataAccess.Repository;
using GP.DataAccess.Repository.BlogRepository;

namespace GP.Application.Commands.BlogCommands.UpdateBlogViewCount
{
    public class UpdateBlogViewCountCommandHandler : ICommandHandler<UpdateBlogViewCountCommand, UpdateBlogViewCountResponse>
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IUnitOfWork _unitOfWork;


        public UpdateBlogViewCountCommandHandler(IBlogRepository blogRepository, IUnitOfWork unitOfWork)
        {
            _blogRepository = blogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateBlogViewCountResponse> Handle(UpdateBlogViewCountCommand command, CancellationToken cancellationToken)
        {
            var id = command.Request.Id;
            
            var blog = await _blogRepository.GetFirstAsync(b => b.Id == id);
            blog.ViewCount++;
            
            _blogRepository.Update(blog);

            await _unitOfWork.CompleteAsync();

            return new UpdateBlogViewCountResponse
            {
            };
        }
    }
}







