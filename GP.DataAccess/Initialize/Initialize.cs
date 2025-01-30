using GP.Core.Enums.Enitity;
using GP.Data;
using GP.DataAccess.Initialize.Data;
using GP.Domain.Entities.Identity;
using GP.Domain.Entities.Lang;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GP.DataAccess.Initialize
{
    public static class Initialize
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            await context.Database.MigrateAsync();
            await context.Database.EnsureCreatedAsync();

            await SeedLanguagesAsync(context).ConfigureAwait(false);
            await SeedKeysAsync(context).ConfigureAwait(false);
            await SeedPermissionAsync(context).ConfigureAwait(false);
            await SeedPermissionCategoryAsync(context).ConfigureAwait(false);
            await SeedUserDataAsync(userManager).ConfigureAwait(false);
            await SeedRoleDataAsync(roleManager).ConfigureAwait(false);
            await JoinUserRoleAsync(userManager, context).ConfigureAwait(false);
            await JoinRolePermissionAsync(roleManager, context).ConfigureAwait(false);
            await JoinUserDirectivePermissionAsync(userManager, context).ConfigureAwait(false);
            await SeedAppDomain(context).ConfigureAwait(false);

            await MigrationLanguageKeyValue(context);
        }

        private static async Task SeedPermissionAsync(ApplicationDbContext context)
        {
            var data = InitializeData.BuildPermissionsList();
            var dbData = context.Permissions.ToList();
            var diffData = data.Where(c => dbData.All(e => e.Label != c.Label)).ToList();
            await context.Permissions.AddRangeAsync(diffData).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedLanguagesAsync(ApplicationDbContext context)
        {
            var data = InitializeData.BuildLanguageList();
            var dbData = context.Languages;
            var diffData = data.Where(c => dbData.All(e => e.Code != c.Code)).ToList();
            await context.Languages.AddRangeAsync(diffData).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedKeysAsync(ApplicationDbContext context)
        {
            var data = InitializeData.BuildKeyList();
            var dbData = context.Keys.Include(k => k.Children);
            var diffData = new List<Key>();

            foreach (var key in data)
            {
                var existingParent = dbData.FirstOrDefault(e => e.Label == key.Label);

                if (existingParent == null)
                {
                    diffData.Add(key);
                }
                else
                {
                    foreach (var child in key.Children)
                    {
                        var existingChild = existingParent.Children.FirstOrDefault(c => c.Label == child.Label);
                        if (existingChild == null)
                        {
                            existingParent.Children.Add(child);
                        }
                    }
                }
            }

            await context.Keys.AddRangeAsync(diffData).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedPermissionCategoryAsync(ApplicationDbContext context)
        {
            var data = InitializeData.BuildPermissionCategories(context);
            var dbData = context.PermissionCategories;
            var diffData = data.Where(c => dbData.All(e => e.Label != c.Label)).ToList();
            await context.PermissionCategories.AddRangeAsync(diffData).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedUserDataAsync(UserManager<User> userManager)
        {
            var defaultPassword = "Aa12345!";

            var data = InitializeData.BuildUserList();
            var dbData = userManager.Users.ToList();
            var diffData = data.Where(c => dbData.All(e => e.UserName != c.UserName)).ToList();
            foreach (var user in diffData)
            {
                await userManager.CreateAsync(user, defaultPassword).ConfigureAwait(false);
            }
        }

        private static async Task SeedRoleDataAsync(RoleManager<Role> roleManager)
        {
            var data = InitializeData.BuildRoleList();
            var dbData = roleManager.Roles.ToList();
            var diffData = data.Where(c => dbData.All(e => e.Name != c.Name)).ToList();
            foreach (var role in diffData)
            {
                await roleManager.CreateAsync(role).ConfigureAwait(false);
            }
        }

        private static async Task JoinUserRoleAsync(UserManager<User> userManager, ApplicationDbContext context)
        {
            if (userManager.Users.Any() && !context.UserRoles.Any())
            {
                await userManager
                    .AddToRoleAsync(await userManager.FindByNameAsync("admin").ConfigureAwait(false), "Admin")
                    .ConfigureAwait(false);
            }
        }

        private static async Task JoinRolePermissionAsync(RoleManager<Role> roleManager, ApplicationDbContext context)
        {
            if (roleManager.Roles.Any())
            {
                var adminRole = await roleManager.Roles.Include(c => c.PermissionCategory)
                    .ThenInclude(c => c.PermissionCategoryPermission).FirstOrDefaultAsync(c => c.Name == "Admin")
                    .ConfigureAwait(false);
                var allPermission = context.PermissionCategoryPermissions.ToList();
                var dbData = adminRole.PermissionCategory.Select(c => c.PermissionCategoryPermission).ToList();
                var diffData = allPermission.Where(c => dbData.All(e => e.Id != c.Id)).ToList();
                foreach (var permission in diffData)
                {
                    adminRole.PermissionCategory.Add(new RolePermissionCategory()
                    {
                        PermissionCategoryPermission = permission
                    });
                }
            }

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task JoinUserDirectivePermissionAsync(UserManager<User> userManager,
            ApplicationDbContext context)
        {
            if (userManager.Users.Any())
            {
                var directivePermissions = InitializeData.BuildUserDirectivePermissionsList(context);

                var adminArr = new string[] { "admin" };
                var admins = userManager.Users.Include(c => c.DirectivePermissions)
                    .Where(c => adminArr.Contains(c.UserName));
                foreach (var admin in admins)
                {
                    var dbData = admin.DirectivePermissions;
                    var diffData = directivePermissions.Where(c => dbData.All(e => e.PermissionId != c.PermissionId))
                        .ToList();
                    foreach (var permission in diffData)
                    {
                        admin.DirectivePermissions.Add(permission);
                    }
                }
            }

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task SeedAppDomain(
            ApplicationDbContext context)
        {
            var data = InitializeData.BuildAppDomainList();
            var dbData = context.AppDomains;
            var diffData = data.Where(c => dbData.All(e => e.Domain != c.Domain)).ToList();
            await context.AppDomains.AddRangeAsync(diffData).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private static async Task MigrationLanguageKeyValue(ApplicationDbContext context)
        {
            var dbKeys = await context.Keys.Where(c => c.Status == RecordStatusEnum.Active)
                .IncludeAll("Children", "Languages", "Parent").ToListAsync();
            var langKeys = await context.LanguageKeys.Where(c => c.Status == RecordStatusEnum.Active)
                .IncludeAll("Key.Parent", "Key.Children", "Language").ToListAsync();

            var languageKeyValues = await context.LanguageKeyValues.ToListAsync();
            var result = Migration();

            List<LanguageKeyValue> Migration()
            {
                var languageKeyValueList = new List<LanguageKeyValue>();
                foreach (var languageKey in langKeys)
                {
                    var keyLabel = FindKeyString(languageKey.Key);
                    if (languageKeyValues.All(c => c.Key != keyLabel))
                    {
                        var data = new LanguageKeyValue()
                        {
                            Value = languageKey.Value,
                            LanguageCode = languageKey.LanguageId,
                            Key = keyLabel,
                        };
                        languageKeyValueList.Add(data);
                    }

                }

                return languageKeyValueList;
            }

            string FindKeyString(Key key)
            {
                //addRow
                var title = key.Label;

                if (key.Parent != null)
                {
                    var parentTitle = FindKeyString(key.Parent);
                    title = $"{parentTitle}.{title}";
                }

                return title;
            }

            await context.LanguageKeyValues.AddRangeAsync(result);
            await context.SaveChangesAsync();
        }
    }
}
