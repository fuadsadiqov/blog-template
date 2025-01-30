using AppDomain = GP.Domain.Entities.App.AppDomain;

namespace GP.DataAccess.Initialize.Data
{
    public static partial class InitializeData
    {
        public static List<AppDomain> BuildAppDomainList()
        {
            var appDomains = new List<AppDomain>()
            {
                new AppDomain()
                {
                    Domain = "aih.az",
                },
                new AppDomain()
                {
                    Domain = "aih.local",
                },
            };

            return appDomains;
        }
    }
}
