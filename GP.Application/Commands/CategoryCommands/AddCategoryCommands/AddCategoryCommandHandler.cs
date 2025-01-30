using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository;
using GP.Core.Enums.Enitity;
using GP.DataAccess.Repository.CategoryRepository;
using GP.Domain.Entities.Common;

namespace GP.Application.Commands.CategoryCommands.AddCategoryCommands
{
    public class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand, AddCategoryResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;


        public AddCategoryCommandHandler(ICategoryRepository categoryRepository,
            IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddCategoryResponse> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
        {
            var title = command.Request.Title;

            await _categoryRepository.AddAsync(new Category()
            {
                Title = title,
                Status = RecordStatusEnum.Active,
                DateCreated = DateTime.Now,
            });

            await _unitOfWork.CompleteAsync();

            return new AddCategoryResponse
            {
            };
        }
    }
}







