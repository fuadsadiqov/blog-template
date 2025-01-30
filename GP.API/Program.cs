using GP.API.HostedServices;
using GP.Application;
using GP.Core.Enums;
using GP.Core.Models;
using GP.Core.Resources;
using GP.Data;
using GP.DataAccess.Initialize;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Middlewares;
using GP.Logging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "VİS API", Version = "v1" });
    swagger.EnableAnnotations();

    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //swagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));

    // To Enable authorization using Swagger (JWT)  
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}

        }
    });
});

// Add services to the container.
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

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (!app.Environment.IsProduction())
{
    app.UseSwagger(c =>
    {
        c.SerializeAsV2 = true;
        c.RouteTemplate = "api/swagger/{documentName}/swagger.json";

    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "GP.API v1");
        c.RoutePrefix = "api/swagger";
    });
    app.UseHsts();
}

app.UseCors();
app.UseHttpsRedirection();

var localizeOptions = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizeOptions.Value);

app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
// app.ConfigureExceptionHandlingMiddleware();
app.ConfigureAutoWrapperMiddleware();
app.ConfigureLoggingMiddleware();
app.UseMiddleware<UserJwtValidatorsMiddleware>();


/*app.MapControllers();
app.MapHub<ConnectionHub>("/hubs/connections",
    o => o.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets);

app.UseUploadFile("/api/upload/file");*/



app.MapControllers();

app.Run();