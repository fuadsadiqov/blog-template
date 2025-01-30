using AutoMapper;
using Microsoft.EntityFrameworkCore;
using GP.Infrastructure.Configurations.Queries;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository.BlogTagRepository;
using GP.Domain.Entities.Common;

namespace GP.Application.BlogTagQueries.GetAllBlogTagsQuery
{
    public class GetAllProductTagsQueryHandler : IQueryHandler<GetAllBlogTagsQuery, GetAllBlogTagsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBlogTagRepository _repository;
        private readonly ExceptionService _exceptionService;

        public GetAllProductTagsQueryHandler(IMapper mapper, IBlogTagRepository repository, ExceptionService exceptionService)
        {
            _exceptionService = exceptionService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetAllBlogTagsResponse> Handle(GetAllBlogTagsQuery query, CancellationToken cancellationToken)
        {
            List<string> includes = new List<string>() { "Tag", "Blog"};
            var blogTags = await _repository.GetAll(includes.ToArray()).ToListAsync(cancellationToken);

            var result = _mapper.Map<List<BlogTag>, List<BlogTagResponse>>(blogTags);
            return new GetAllBlogTagsResponse()
            {
                BlogTagResponses = result,
            };
        }
    }
}
