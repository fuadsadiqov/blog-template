using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository;
using GP.Core.Enums.Enitity;
using GP.DataAccess.Repository.TagRepository;
using GP.Core.Exceptions;
using GP.DataAccess;
using GP.DataAccess.Repository.BlogTagRepository;
using GP.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

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
            
            var blogTags = _blogTagRepository.FindBy(bT => bT.BlogId == blogId).ToList();

            if (blogTags.Count > 0)
            {
                var blogTagsToDelete = blogTags.Where(bT => !tagIds.Contains(bT.TagId)).ToList();

                foreach (var blogTagToDelete in blogTagsToDelete)
                {
                    _blogTagRepository.Delete(blogTagToDelete, false);
                }
            }
            
            foreach (var tagId in tagIds)
            {
                var tag = await _tagRepository.GetFirstAsync(x => x.Id == tagId);
                if (tag is null)
                {
                    throw new RecordNotFoundException("Tag is not exist");
                }
                
                var tagExist = await _blogTagRepository.GetFirstAsync(bT => (bT.TagId == tagId && bT.BlogId == blogId));
                if(tagExist is null)
                {
                    var blogTag = new BlogTag()
                    {
                        TagId = tagId,
                        BlogId = blogId,
                        Status = RecordStatusEnum.Active,
                        DateCreated = DateTime.Now
                    };
                    await _blogTagRepository.AddAsync(blogTag);
                }
            }

            await _unitOfWork.CompleteAsync();

            return new SetBlogTagResponse
            {
            };
        }
    }
}







