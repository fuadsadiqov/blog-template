using GP.Core.Attributes;
using GP.Core.Configurations.Entity;
using GP.Core.Enums;
using GP.Core.Models;
using System.Linq.Dynamic.Core;

namespace GP.Core.Extensions
{
    public static class EntitySortingExtensions
    {
        public static List<SortParameters> NormalizeLocalizeEntityTitleSort(this List<SortParameters> sp, string acceptLanguage, string propertyPrefix = "")

        {
            if (sp != null && sp.Any())
                sp.ForEach(c => c.NormalizeLocalizeEntityTitleSort(acceptLanguage, propertyPrefix));
            return sp;
        }
        public static List<SortParameters> NormalizeLocalizeSort<T>(this List<SortParameters> sp, string acceptLanguage)

        {
            if (sp != null && sp.Any())
                sp.ForEach(c => c.NormalizeLocalizeSort<T>(acceptLanguage));
            return sp;
        }
        public static SortParameters NormalizeLocalizeEntityTitleSort(this SortParameters sp, string acceptLanguage, string propertyPrefix = "")

        {
            var prop = "";
            switch (acceptLanguage)
            {
                case LanguageStringEnum.en_US:
                    prop = nameof(ILocalizeEntity.TitleEn);
                    break; ;
                case LanguageStringEnum.az_AZ:
                    prop = nameof(ILocalizeEntity.TitleAz);
                    break;
                default:
                    prop = nameof(ILocalizeEntity.TitleEn);
                    break;
            }

            if (sp != null && sp.Prop.StartsWith(propertyPrefix, StringComparison.InvariantCultureIgnoreCase))
            {
                sp.Prop = sp.Prop.Replace("title", prop, StringComparison.InvariantCultureIgnoreCase);

            }

            return sp;
        }
        public static SortParameters NormalizeLocalizeSort<T>(this SortParameters sp, string acceptLanguage)

        {
            const string en = LanguageStringEnum.en_US;

            var languageEnum = acceptLanguage == en ? LanguageEnum.En : LanguageEnum.Az;
            var type = typeof(T);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var localizeEntityAttribute = Attribute.GetCustomAttribute(property, typeof(LocalizeAttribute)) as LocalizeAttribute;

                if (localizeEntityAttribute != null && sp.Prop.ToLower() == localizeEntityAttribute.Key.ToLower() && localizeEntityAttribute.LanguageEnum == languageEnum)
                {
                    sp.Prop = property.Name;
                    break;
                }
            }

            return sp;
        }
        private static string ToOrderString(this List<SortParameters> sp)
        {
            return String.Join(",", sp.Select(c => c.ToOrderString()).ToArray()); ;
        }
        private static string ToOrderString(this SortParameters sp)
        {
            return $"{sp.Prop} {sp.Dir}";
        }
        public static IQueryable<T> SortBy<T>(this IQueryable<T> query, SortParameters sortParameters)
        {

            List<SortParameters> sortParametersList = new List<SortParameters>();
            if (sortParameters != null && !string.IsNullOrEmpty(sortParameters.Prop) && !string.IsNullOrEmpty(sortParameters.Dir))
            {
                if (sortParameters.Prop != "Id")
                    sortParametersList.Add(sortParameters);
            }

            sortParametersList.Add(new SortParameters()
            {
                Prop = "Id",
                Dir = "asc"
            });

            var orderString = sortParametersList.ToOrderString();
            query = query.OrderBy(orderString);
            return query;

        }
        public static IQueryable<T> SortBy<T>(this IQueryable<T> query, List<SortParameters> sortParameterList)
        {
            if (sortParameterList == null || !sortParameterList.Any())
            {
                sortParameterList ??= new List<SortParameters>();
                sortParameterList.Add(new SortParameters()
                {
                    Prop = "Id",
                    Dir = "desc"
                });
            }
            query = query.OrderBy(sortParameterList.ToOrderString());
            return query;
        }
        public static IEnumerable<T> SortBy<T>(this IEnumerable<T> query, SortParameters sortParameters)
        {

            List<SortParameters> sortParametersList = new List<SortParameters>();
            if (sortParameters != null && !string.IsNullOrEmpty(sortParameters.Prop) && !string.IsNullOrEmpty(sortParameters.Dir))
            {
                if (sortParameters.Prop != "Id")
                    sortParametersList.Add(sortParameters);
            }

            sortParametersList.Add(new SortParameters()
            {
                Prop = "Id",
                Dir = "asc"
            });

            var orderString = sortParametersList.ToOrderString();
            query = query.AsQueryable().OrderBy(orderString);
            return query;

        }
        public static IEnumerable<T> SortBy<T>(this IEnumerable<T> query, List<SortParameters> sortParameterList)
        {
            if (sortParameterList == null || !sortParameterList.Any())
            {
                sortParameterList ??= new List<SortParameters>();
                sortParameterList.Add(new SortParameters()
                {
                    Prop = "Id",
                    Dir = "desc"
                });
            }
            query = query.AsQueryable().OrderBy(sortParameterList.ToOrderString());
            return query;
        }
    }
}
