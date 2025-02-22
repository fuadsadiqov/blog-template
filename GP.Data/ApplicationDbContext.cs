using GP.Core.Enums.Enitity;
using GP.Data.Configurations;
using GP.Domain.Entities.App;
using GP.Domain.Entities.Audit;
using GP.Domain.Entities.Common;
using GP.Domain.Entities.Identity;
using GP.Domain.Entities.Lang;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using AppDomain = GP.Domain.Entities.App.AppDomain;


namespace GP.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,
        Role, string, UserClaim,
        UserRole,
        UserLogin,
        RoleClaim,
        UserToken>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        #region Identity

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<PermissionCategory> PermissionCategories { get; set; }
        public DbSet<PermissionCategoryPermission> PermissionCategoryPermissions { get; set; }
        public DbSet<RolePermissionCategory> RolePermissionCategories { get; set; }
        public DbSet<UserJwt> UserJwts { get; set; }

        #endregion

        #region Language

        public DbSet<Language> Languages { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<LanguageKey> LanguageKeys { get; set; }
        public DbSet<LanguageKeyValue> LanguageKeyValues { get; set; }

        #endregion

        public DbSet<SmsSenderServiceLog> SmsSenderServiceLogs { get; set; }

        #region App
        public DbSet<AppOption> AppOptions { get; set; }
        public DbSet<AppDomain> AppDomains { get; set; }
        #endregion

        #region Common
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<Review> Reviews { get; set; }
        #endregion
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RolePermissionCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageKeyConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageKeyValueConfiguration());
            modelBuilder.ApplyConfiguration(new BlogTagConfiguration());

            modelBuilder.Entity<User>().ToTable("Users", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<Role>().ToTable("Roles", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<UserRole>().ToTable("UserRoles", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<UserToken>().ToTable("UserTokens", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<UserJwt>().ToTable("UserJwts", SchemaNames.IdentitySchemaName);


            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<RolePermissionCategory>()
                .ToTable("RolePermissionCategories", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<UserPermission>().ToTable("UserPermissions", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<Permission>().ToTable("Permissions", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<PermissionCategory>().ToTable("PermissionCategories", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<PermissionCategoryPermission>()
                .ToTable("PermissionCategoryPermissions", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<EmailConfirmationRequest>()
                .ToTable("EmailConfirmationRequests", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<UserSmsConfirmationRequest>()
                .ToTable("SmsConfirmationRequests", SchemaNames.IdentitySchemaName);
            modelBuilder.Entity<PasswordResetRequest>()
                .ToTable("PasswordResetRequests", SchemaNames.IdentitySchemaName);

            modelBuilder.Entity<SmsSenderServiceLog>().ToTable("SmsSenderServiceLogs", SchemaNames.LogSchemaName);

            modelBuilder.Entity<Language>().ToTable("Languages", SchemaNames.LanguageSchemaName);
            modelBuilder.Entity<Key>().ToTable("Keys", SchemaNames.LanguageSchemaName);
            modelBuilder.Entity<LanguageKey>().ToTable("LanguageKeys", SchemaNames.LanguageSchemaName);
            modelBuilder.Entity<LanguageKeyValue>().ToTable("LanguageKeyValues", SchemaNames.LanguageSchemaName);


            modelBuilder.Entity<AppOption>().ToTable("AppOptions", SchemaNames.AppSchemaName);
            modelBuilder.Entity<AppDomain>().ToTable("AppDomains", SchemaNames.AppSchemaName);

            modelBuilder.Entity<Blog>().ToTable("Blogs", SchemaNames.CommonSchemaName);
            modelBuilder.Entity<Tag>().ToTable("Tags", SchemaNames.CommonSchemaName);
            modelBuilder.Entity<BlogTag>().ToTable("BlogTags", SchemaNames.CommonSchemaName);
            modelBuilder.Entity<Category>().ToTable("Categories", SchemaNames.CommonSchemaName);
            modelBuilder.Entity<Review>().ToTable("Reviews", SchemaNames.CommonSchemaName);

            modelBuilder.SetStatusQueryFilter();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var entry in ChangeTracker.Entries())
            {
                var propertyNames = entry.Properties.Select(c => c.Metadata.Name).ToList();

                switch (entry.State)
                {
                    case EntityState.Added:
                        if (propertyNames.Any(c => c == "DateCreated"))
                            entry.CurrentValues["DateCreated"] = DateTime.Now;

                        if (propertyNames.Any(c => c == "CreatedById"))
                            entry.CurrentValues["CreatedById"] = userId;

                        if (propertyNames.Any(c => c == "Status"))
                            entry.CurrentValues["Status"] = RecordStatusEnum.Active;
                        break;
                    case EntityState.Modified:
                        if (propertyNames.Any(c => c == "DateModified"))
                            entry.CurrentValues["DateModified"] = DateTime.Now;

                        if (propertyNames.Any(c => c == "ModifiedById"))
                            entry.CurrentValues["ModifiedById"] = userId;
                        break;
                }
            }
        }
    }
}