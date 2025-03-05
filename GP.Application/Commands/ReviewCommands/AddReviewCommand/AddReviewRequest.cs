using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.Application.Commands.ReviewCommands.AddReviewCommand
{
    public class AddReviewRequest
    {
        public string Message{ get; set; }
        public string Email{ get; set; }
        public Guid BlogId { get; set; }
    }
}
