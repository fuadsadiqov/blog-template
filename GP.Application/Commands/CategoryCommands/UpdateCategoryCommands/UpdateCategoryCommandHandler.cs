using GP.DataAccess.Repository.UserRepository;
using GP.Infrastructure.Configurations.Commands;
using GP.Infrastructure.Services;
using GP.DataAccess.Repository;
using GP.Core.Enums.Enitity;
using GP.DataAccess.Repository.CategoryRepository;
using GP.Domain.Entities.Common;

namespace GP.Application.Commands.CategoryCommands.UpdateCategoryCommands
{
    public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, UpdateCategoryResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ExceptionService _exceptionService;
        private readonly IUnitOfWork _unitOfWork;


        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository,
            IUserRepository userRepository, ExceptionService exceptionService, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _exceptionService = exceptionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var id = command.Request.Id;
            var title = command.Request.Title;
            var category = await _categoryRepository.GetFirstAsync(c => c.Id == id);
            category.Title = title;
            category.DateModified = DateTime.Now;
            
            _categoryRepository.Update(category);

            await _unitOfWork.CompleteAsync();

            return new UpdateCategoryResponse
            {
            };
        }
    }
}







