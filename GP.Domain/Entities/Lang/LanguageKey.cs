using GP.Domain.Common.Configurations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GP.Domain.Entities.Lang
{
    public class LanguageKey : Entity
    {
        [Key]
        [ForeignKey("Language")]
        public string LanguageId { get; set; }
        [Key]
        [ForeignKey("Key")]
        public int KeyId { get; set; }
        public Language Language { get; set; }
        public Key Key { get; set; }
        public string Value { get; set; }
    }
}
