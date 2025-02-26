using GP.Application;
using GP.Core.Enums;
using GP.Core.Models;
using GP.Domain.Entities.Identity;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;
using GP.API.HostedServices;
using GP.Core.Resources;
using Serilog;
using GP.Logging;
using GP.Data;
using GP.DataAccess.Initialize;
using GP.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(session =>
{
    session.IdleTimeout = TimeSpan.FromMinutes(1);
    session.Cookie.HttpOnly = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(googleOptions =>
{
    //googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientId = "1023033195510-t3f86pfrb040p631kn2okhpukm50dctv.apps.googleusercontent.com";
    //googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = "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";
    googleOptions.CallbackPath = "/signin-google";
    googleOptions.Events.OnCreatingTicket = ctx =>
    {
        var identity = (ClaimsIdentity)ctx.Principal.Identity;
        var email = ctx.User.GetProperty("email").GetString();
        var name = ctx.User.GetProperty("name").GetString();
        identity.AddClaim(new Claim(ClaimTypes.Email, email));
        identity.AddClaim(new Claim(ClaimTypes.Name, name));
        return Task.CompletedTask;
    };

});

builder.Services.AddResponsiveFileManager(options =>
{
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Configuration.AddJsonFile("Settings/smtpSettings.json", false, true);
builder.Configuration.AddJsonFile($"Settings/smtpSettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
    optional: true);
builder.Configuration.AddJsonFile("Settings/authSettings.json", false, true);
builder.Configuration.AddJsonFile("Settings/appsettings.json", false, true);
builder.Configuration.AddJsonFile($"Settings/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
    optional: true);
builder.Configuration.AddJsonFile("Settings/localIpAddressSettings.json", false, true);
builder.Configuration.AddJsonFile("Settings/tokenSettings.json", false, true);
builder.Configuration.AddJsonFile($"Settings/tokenSettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
    optional: true);
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true, true);

builder.Services.AddLocalization();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
if (builder.Environment.IsStaging())
{
    builder.Configuration.AddUserSecrets("staging-environment-secrets");
}

builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddCommandLine(args);


builder.Services.Configure<RequestLocalizationOptions>(
            options =>
            {
                var enUsCulture = new CultureInfo(LanguageStringEnum.en_US);
                var azCulture = new CultureInfo(LanguageStringEnum.az_AZ)
                {
                    NumberFormat = enUsCulture.NumberFormat,
                    DateTimeFormat = enUsCulture.DateTimeFormat
                };

                var supportedCultures = new List<CultureInfo>
                {
                    enUsCulture,
                    azCulture
                };

                options.DefaultRequestCulture = new RequestCulture(culture: LanguageStringEnum.en_US, uiCulture: LanguageStringEnum.en_US);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });


builder.Services.AddControllers().AddDataAnnotationsLocalization(
            options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(Resource));
            }).AddNewtonsoftJson();

builder.Services.AddSignalR();

builder.Services.Configure<ApiBehaviorOptions>(
            options =>
            {
                options.SuppressModelStateInvalidFilter = true; // set Modelstate validation manual

            });
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("tokenSettings"));
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("authSettings"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("smtpSettings"));
builder.Services.Configure<LocalIpAddressSettings>(builder.Configuration.GetSection("localIpAddressSettings"));

builder.Services.ConfigureDependencyInjections(builder.Configuration, new DependencyInjectionOptions()
{
    AutoMapperAssemblies = new Assembly[]
                    {
                        typeof(Program).Assembly,
                        typeof(DependencyInjection).Assembly,
                        typeof(GP.Infrastructure.Services.AuthService).Assembly,
                    }
});
builder.Services.AddHostedService<CacheDataRestorationService>();

Log.Logger = new LoggerConfiguration().BuildLoggerConfiguration(builder.Services, builder.Configuration).CreateLogger();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
    await Initialize.SeedAsync(context, userManager, roleManager).ConfigureAwait(false);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors();
app.UseHttpsRedirection();

app.UseSession();

var localizeOptions = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizeOptions.Value);

app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
// app.ConfigureExceptionHandlingMiddleware();
app.ConfigureAutoWrapperMiddleware();
app.ConfigureLoggingMiddleware();
app.UseMiddleware<UserJwtValidatorsMiddleware>();

app.UseResponsiveFileManager();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapAreaControllerRoute(
    name : "areas",
    areaName: "Home",
    pattern : "{controller=Home}/{action=Index}/{id?}"
);

app.MapAreaControllerRoute(
    name : "areas",
    areaName: "Admin",
    pattern : "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapAreaControllerRoute(
    name : "areas",
    areaName: "Account",
    pattern : "{area:exists}/{controller=Login}/{action=Index}/{id?}"
);

app.Run();
