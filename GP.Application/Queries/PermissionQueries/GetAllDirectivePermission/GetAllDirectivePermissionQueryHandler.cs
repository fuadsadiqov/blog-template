using AutoMapper;
using GP.DataAccess.Repository.PermissionRepository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Configurations.Queries;
using Microsoft.EntityFrameworkCore;

namespace GP.Application.Queries.PermissionQueries.GetAllDirectivePermission
{
    public class GetAllDirectivePermissionQueryHandler : IQueryHandler<GetAllDirectivePermissionQuery, GetAllDirectivePermissionResponse>
    {
        private readonly IPermissionRepository _repository;
        private readonly IMapper _mapper;

        public GetAllDirectivePermissionQueryHandler(IPermissionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetAllDirectivePermissionResponse> Handle(GetAllDirectivePermissionQuery query, CancellationToken cancellationToken)
        {
            var data = _repository.FindBy(c => c.IsDirective && !string.Equals(c.Label.ToLower(), "admin"));
            data = data.OrderByDescending(c => c.Label);
            var result =
                _mapper.Map<List<Permission>, List<PermissionResponse>>(await data.ToListAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(false));

            return new GetAllDirectivePermissionResponse()
            {
                Response = result
            };
        }
    }
}
