using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository;
using GP.Core.Enums.Enitity;
using GP.DataAccess.Repository.TagRepository;
using GP.Core.Exceptions;
using GP.DataAccess.Repository.BlogTagRepository;
using GP.Domain.Entities.Common;

namespace GP.Application.Commands.BlogCommands.SetBlogTag
{
    public class SetBlogTagCommandHandler : ICommandHandler<SetBlogTagCommand, SetBlogTagResponse>
    {
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;


        public SetBlogTagCommandHandler(IBlogTagRepository blogTagRepository, ITagRepository tagRepository,
            ExceptionService exceptionService, IUnitOfWork unitOfWork)
        {
            _blogTagRepository = blogTagRepository;
            _tagRepository = tagRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<SetBlogTagResponse> Handle(SetBlogTagCommand command, CancellationToken cancellationToken)
        {

            var tagIds = command.Request.TagIds;
            var blogId = command.Request.BlogId;

            foreach (var tagId in tagIds)
            {
                var tagExist = await _tagRepository.GetFirstAsync(t => t.Id == tagId);
                if(tagExist is null)
                {
                    throw new RecordNotFoundException("Id " + tagId + " not found");
                }

                var blogTag = new BlogTag()
                {
                    TagId = tagExist.Id,
                    BlogId = blogId,
                    Status = RecordStatusEnum.Active,
                    DateCreated = DateTime.Now
                };
                await _blogTagRepository.AddAsync(blogTag);
            }

            await _unitOfWork.CompleteAsync();

            return new SetBlogTagResponse
            {
            };
        }
    }
}







