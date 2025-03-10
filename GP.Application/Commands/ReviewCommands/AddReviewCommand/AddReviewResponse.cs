using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.Application.Commands.ReviewCommands.AddReviewCommand
{
    public class AddReviewResponse
    {
        public bool IsSuccedd { get; set; }
        public string? Message { get; set; } = null;
    }
}
