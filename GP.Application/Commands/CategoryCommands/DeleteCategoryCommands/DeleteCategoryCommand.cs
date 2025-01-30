using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.CategoryCommands.DeleteCategoryCommands
{
    public class DeleteCategoryCommand : CommandBase<DeleteCategoryResponse>
    {
        public DeleteCategoryCommand(DeleteCategoryRequest request)
        {
            Request = request;
        }
        public DeleteCategoryRequest Request { get; set; }
    }
}
