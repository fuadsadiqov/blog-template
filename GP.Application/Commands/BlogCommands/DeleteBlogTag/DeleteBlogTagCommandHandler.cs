using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository;
using MediatR;
using GP.DataAccess.Repository.BlogTagRepository;

namespace GP.Application.Commands.BlogCommands.DeleteBlogTag
{
    public class DeleteBlogTagCommandHandler : ICommandHandler<DeleteBlogTagCommand, DeleteBlogTagResponse>
    {
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;


        public DeleteBlogTagCommandHandler(IBlogTagRepository blogTagRepository,
            IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _blogTagRepository = blogTagRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<DeleteBlogTagResponse> Handle(DeleteBlogTagCommand command, CancellationToken cancellationToken)
        {
            //var productId = command.Request.Id;

            //var productTags = await _productTagRepository.FindBy(pT => pT.ProductId == productId).ToListAsync(cancellationToken);

            //foreach (var productTag in productTags)
            //{
            //    _productTagRepository.Delete(productTag);
            //}

            await _unitOfWork.CompleteAsync();

            return new DeleteBlogTagResponse
            {
            };
        }
    }
}







