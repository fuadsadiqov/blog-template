using GP.Domain.Entities.Lang;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GP.Data.Configurations
{
    public class LanguageKeyConfiguration : IEntityTypeConfiguration<LanguageKey>
    {
        public void Configure(EntityTypeBuilder<LanguageKey> modelBuilder)
        {
            modelBuilder.HasKey(ul => new { ul.LanguageId, ul.KeyId });

        }
    }
    public class LanguageKeyValueConfiguration : IEntityTypeConfiguration<LanguageKeyValue>
    {
        public void Configure(EntityTypeBuilder<LanguageKeyValue> modelBuilder)
        {
            modelBuilder.HasKey(ul => new { ul.LanguageCode, ul.Key });

        }
    }
}
