using GP.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GP.Data.Configurations
{
    public class RolePermissionCategoryConfiguration : IEntityTypeConfiguration<RolePermissionCategory>
    {
        public void Configure(EntityTypeBuilder<RolePermissionCategory> modelBuilder)
        {
            modelBuilder.HasKey(table => new
            {
                table.RoleId,
                table.PermissionCategoryPermissionId
            });
            modelBuilder.Property(x => x.RoleId).IsRequired().HasMaxLength(128);

        }
    }
}
