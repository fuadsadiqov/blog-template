using AutoMapper;
using Microsoft.EntityFrameworkCore;
using GP.Infrastructure.Configurations.Queries;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository.CategoryRepository;
using GP.Domain.Entities.Common;

namespace GP.Application.CategoryQueries.GetAllCategoriesQuery
{
    public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, GetAllCategoriesResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _repository;
        private readonly ExceptionService _exceptionService;

        public GetAllCategoriesQueryHandler(IMapper mapper, ICategoryRepository repository, ExceptionService exceptionService)
        {
            _exceptionService = exceptionService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetAllCategoriesResponse> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
        {
            var categories =await _repository.GetAll().ToListAsync(cancellationToken);

            var result = _mapper.Map<List<Category>, List<CategoryResponse>>(categories);
            return new GetAllCategoriesResponse()
            {
                CategoryResponses = result,
            };
        }
    }
}
