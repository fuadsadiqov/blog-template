using GP.Data;
using GP.Domain.Entities.Audit;

namespace GP.DataAccess.Repository.SmsSenderServiceLogRepository
{
    public class SmsSenderServiceLogRepository
        : Repository<SmsSenderServiceLog>,
        ISmsSenderServiceLogRepository,
        IRepositoryIdentifier
    {
        public SmsSenderServiceLogRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
