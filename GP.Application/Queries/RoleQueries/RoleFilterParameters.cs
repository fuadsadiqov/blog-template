using GP.Core.Models;

namespace GP.Application.Queries.RoleQueries
{
    public class RoleFilterParameters : BaseFilterParameters
    {
        public string Text { get; set; }
        public List<string> Ids { get; set; }
    }
}
