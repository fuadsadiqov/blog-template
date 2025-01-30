using System.ComponentModel.DataAnnotations;
using GP.Core.Resources;

namespace GP.Application.Commands.BlogCommands.SetBlogTag
{
    public class SetBlogTagRequest
    {
        public Guid BlogId { get; set; } = Guid.Empty;
        public List<Guid> TagIds { get; set; }
    }
}
