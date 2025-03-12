using AutoMapper;
using GP.DataAccess;
using Microsoft.EntityFrameworkCore;
using GP.Infrastructure.Configurations.Queries;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository.BlogTagRepository;
using GP.DataAccess.Repository.BlogRepository;
using GP.Domain.Entities.Common;

namespace GP.Application.BlogQueries.GetAllBlogsQuery
{
    public class GetAllProductsQueryHandler : IQueryHandler<GetAllBlogsQuery, GetAllBlogsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBlogRepository _repository;
        private readonly IBlogTagRepository _productTagRepository;
        private readonly ExceptionService _exceptionService;

        public GetAllProductsQueryHandler(IMapper mapper, IBlogRepository repository, IBlogTagRepository productTagRepository, ExceptionService exceptionService)
        {
            _exceptionService = exceptionService;
            _repository = repository;
            _productTagRepository = productTagRepository;
            _mapper = mapper;
        }

        public async Task<GetAllBlogsResponse> Handle(GetAllBlogsQuery query, CancellationToken cancellationToken)
        {
            var categoryId = query.Request.CategoryId;

            List<string> includes = new List<string>(){ "Category", "Tags.Tag" };
            var blogs = _repository.GetAll(includes.ToArray());
            if (categoryId != null)
            {
                blogs = blogs.Where(b => b.CategoryId == categoryId);
            }
            
            if (!query.Request.PagingParameters.IsAll)
            {
                blogs = blogs.FindPaged(query.Request.PagingParameters);
            }
            
            var result = _mapper.Map<List<Blog>, List<BlogResponse>>(blogs.ToList());
            
            return new GetAllBlogsResponse()
            {
                BlogResponses = result,
            };
        }
    }
}
