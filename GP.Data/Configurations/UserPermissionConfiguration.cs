using GP.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GP.Data.Configurations
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> modelBuilder)
        {
            modelBuilder.HasKey(table => new
            {
                table.UserId,
                table.PermissionId
            });

        }
    }
}
