using GP.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace GP.Application.Queries.LanguageQueries.GetAllLanguageKeyValue
{
    public record GetAllLanguageKeyValueRequest([RegularExpression("az-AZ|en-US", ErrorMessage = ResourceKey.DataIsNotValidFormat)] string LanguageCode);
}
