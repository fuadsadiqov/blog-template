using GP.Infrastructure.Configurations.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.Application.Commands.ReviewCommands.AddReviewCommand
{
    public class AddReviewCommand : CommandBase<AddReviewResponse>
    {
        public AddReviewCommand(AddReviewRequest request) { 
            Request = request;
        }
        public AddReviewRequest Request { get; set; }
    }
}
