using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository;
using GP.DataAccess.Repository.CategoryRepository;
using GP.Core.Exceptions;

namespace GP.Application.Commands.CategoryCommands.DeleteCategoryCommands
{
    public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, DeleteCategoryResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;


        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository,
            IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteCategoryResponse> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            var id = command.Request.Id;

            var category = await _categoryRepository.GetFirstAsync(c => c.Id == id);
            if(category is null)
            {
                throw new RecordNotFoundException("Category not found");
            }
            _categoryRepository.Delete(category);

            await _unitOfWork.CompleteAsync();

            return new DeleteCategoryResponse
            {
            };
        }
    }
}







