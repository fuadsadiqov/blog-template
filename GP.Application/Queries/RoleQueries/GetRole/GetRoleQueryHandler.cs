using GP.Application.Queries.RoleQueries.GetAllRole;
using GP.Infrastructure.Configurations.Queries;
using GP.Infrastructure.Services;
using MediatR;

namespace GP.Application.Queries.RoleQueries.GetRole
{
    public class GetRoleQueryHandler : IQueryHandler<GetRoleQuery, GetRoleResponse>
    {
        private readonly IMediator _mediator;
        private readonly ExceptionService _exceptionService;

        public GetRoleQueryHandler(IMediator mediator, ExceptionService exceptionService)
        {
            _mediator = mediator;
            _exceptionService = exceptionService;
        }

        public async Task<GetRoleResponse> Handle(GetRoleQuery query, CancellationToken cancellationToken)
        {
            var result = (await _mediator.Send(new GetAllRoleQuery(new GetAllRoleRequest()
            {
                FilterParameters = new RoleFilterParameters()
                {
                    Ids = new List<string>() { query.Request.Id }
                }
            }), cancellationToken)).Response.Items.FirstOrDefault();

            if (result == null)
                throw _exceptionService.RecordNotFoundException();

            return new GetRoleResponse()
            {
                Response = result
            };
        }
    }
}
