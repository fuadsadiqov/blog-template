using AutoMapper;
using Microsoft.EntityFrameworkCore;
using GP.Infrastructure.Configurations.Queries;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository.BlogTagRepository;
using GP.DataAccess.Repository.BlogRepository;
using GP.Domain.Entities.Common;
using System.Linq.Expressions;
using GP.DataAccess.Repository;

namespace GP.Application.BlogQueries.GetBlogQuery
{
    public class GetBlogQueryHandler: IQueryHandler<GetBlogQuery, GetBlogResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBlogRepository _repository;
        private readonly IBlogTagRepository _productTagRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;

        public GetBlogQueryHandler(IMapper mapper, IBlogRepository repository, IBlogTagRepository productTagRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork)
        {
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _productTagRepository = productTagRepository;
            _mapper = mapper;
        }

        public async Task<GetBlogResponse> Handle(GetBlogQuery query, CancellationToken cancellationToken)
        {
            List<string> includes = new List<string>(){ "Category", "Tags.Tag"};
            var id = query.Request.Id;
            var blog = await _repository.GetFirstAsync(b => b.Id == id, includes.ToArray());
            var result = _mapper.Map<Blog, BlogResponse>(blog);

            return new GetBlogResponse()
            {
                BlogResponses = result,
            };
        }
    }
}
