using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.CategoryCommands.UpdateCategoryCommands
{
    public class UpdateCategoryCommand : CommandBase<UpdateCategoryResponse>
    {
        public UpdateCategoryCommand(UpdateCategoryRequest request)
        {
            Request = request;
        }
        public UpdateCategoryRequest Request { get; set; }
    }
}
