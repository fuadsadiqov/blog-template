using GP.Domain.Common.Configurations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GP.Domain.Entities.Lang
{
    public class LanguageKeyValue : Entity
    {
        [Key]
        [ForeignKey("Language")]
        public string LanguageCode { get; set; }
        public Language Language { get; set; }
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
