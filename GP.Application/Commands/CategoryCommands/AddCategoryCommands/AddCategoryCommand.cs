using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.CategoryCommands.AddCategoryCommands
{
    public class AddCategoryCommand : CommandBase<AddCategoryResponse>
    {
        public AddCategoryCommand(AddCategoryRequest request)
        {
            Request = request;
        }
        public AddCategoryRequest Request { get; set; }
    }
}
