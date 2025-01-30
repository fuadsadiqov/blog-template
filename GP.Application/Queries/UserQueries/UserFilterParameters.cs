using GP.Core.Enums.Enitity;
using GP.Core.Models;

namespace GP.Application.Queries.UserQueries
{
    public class UserFilterParameters : BaseFilterParameters
    {
        public string Text { get; set; }
        public List<string> Ids { get; set; }
        public List<string> RoleIds { get; set; }
        public RecordStatusEnum? Status { get; set; }
    }
}
