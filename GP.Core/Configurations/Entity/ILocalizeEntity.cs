using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GP.Core.Configurations.Entity
{
    public interface ILocalizeEntity
    {
        //default EN
        [NotMapped]
        public string Title { get; set; }
        [Required]
        public string TitleEn { get; set; }
        public string TitleAz { get; set; }

    }
}
