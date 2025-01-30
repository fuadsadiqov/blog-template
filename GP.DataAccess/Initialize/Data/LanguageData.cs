using GP.Core.Enums;
using GP.Domain.Entities.Lang;

namespace GP.DataAccess.Initialize.Data
{
    public static partial class InitializeData
    {

        public static List<Language> BuildLanguageList()
        {
            var list = new List<Language>
            {
                new Language()
                {
                    Code = LanguageStringEnum.az_AZ , Title = "Azerbaijani"

                }, new Language()
                {
                    Code = LanguageStringEnum.en_US , Title = "English"

                }
            };

            return list;
        }
    }
}
