using GP.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GP.Data.Configurations
{
    public class BlogTagConfiguration : IEntityTypeConfiguration<BlogTag>
    {
        public void Configure(EntityTypeBuilder<BlogTag> modelBuilder)
        {
            modelBuilder.HasKey(pr => new {pr.BlogId, pr.TagId });
            modelBuilder.HasOne(ur => ur.Blog)
                .WithMany(r => r.Tags)
                .HasForeignKey(ur => ur.BlogId)
                .IsRequired();

            modelBuilder.HasOne(ur => ur.Tag)
                .WithMany(r => r.Blogs)
                .HasForeignKey(ur => ur.TagId)
                .IsRequired();

            modelBuilder.Property(x => x.BlogId).IsRequired().HasMaxLength(128);
            modelBuilder.Property(x => x.TagId).IsRequired().HasMaxLength(128);

        }
    }
}
