using GP.Core.Constants;
using GP.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace GP.Application.Commands.LanguageCommands.SetLanguageKeyValue
{
    public record SetLanguageKeyValueRequest(
    [Required(ErrorMessage = ResourceKey.Required)]
    [RegularExpression("az-AZ|en-US",ErrorMessage = ResourceKey.DataIsNotValidFormat)] string LanguageCode,
    [Required(ErrorMessage = ResourceKey.Required)]
    [RegularExpression(
        RegexConstants.LanguageKeyRegex,
        ErrorMessage = ResourceKey.DataIsNotValidFormat)]
    string Key,
    [Required(ErrorMessage = ResourceKey.Required)] string Value);
}
