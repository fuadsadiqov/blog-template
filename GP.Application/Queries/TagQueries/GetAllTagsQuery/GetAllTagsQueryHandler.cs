using AutoMapper;
using Microsoft.EntityFrameworkCore;
using GP.Infrastructure.Configurations.Queries;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository.TagRepository;
using GP.Domain.Entities.Common;

namespace GP.Application.TagQueries.GetAllTagsQuery
{
    public class GetAllTagsQueryHandler : IQueryHandler<GetAllTagsQuery, GetAllTagsResponse>
    {
        private readonly IMapper _mapper;
        private readonly ITagRepository _repository;
        private readonly ExceptionService _exceptionService;

        public GetAllTagsQueryHandler(IMapper mapper, ITagRepository repository, ExceptionService exceptionService)
        {
            _exceptionService = exceptionService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetAllTagsResponse> Handle(GetAllTagsQuery query, CancellationToken cancellationToken)
        {
            var tags = await _repository.GetAll().ToListAsync(cancellationToken);

            var result = _mapper.Map<List<Tag>, List<TagResponse>>(tags);
            return new GetAllTagsResponse()
            {
                TagResponses = result,
            };
        }
    }
}
