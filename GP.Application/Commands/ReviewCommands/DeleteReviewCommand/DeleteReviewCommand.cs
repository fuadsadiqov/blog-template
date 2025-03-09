using GP.Infrastructure.Configurations.Commands;

namespace GP.Application.Commands.ReviewCommands.DeleteReviewCommand;

public class DeleteReviewCommand : CommandBase<DeleteReviewResponse>
{
    public DeleteReviewCommand(DeleteReviewRequest request)
    {
        Request = request;
    }
    public DeleteReviewRequest Request { get; set; }
}