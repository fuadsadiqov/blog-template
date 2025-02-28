using AutoWrapper;
using GP.Core.Exceptions;
using GP.Core.Extensions;
using GP.Core.Models;
using GP.Data;
using GP.DataAccess.Repository;
using GP.Domain.Entities.Identity;
using GP.Infrastructure.Middlewares;
using GP.Infrastructure.Pipelines;
using GP.Infrastructure.Services;
using GP.Infrastructure.Services.SmsConfirmationService;
using GP.Infrastructure.Services.SmsService;
using GP.Logging;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace GP.Application
{
    public class DependencyInjectionOptions
    {
        public Assembly[]? AutoMapperAssemblies { get; set; }
        public Assembly SwaggerAssembly { get; set; }
    }

    public static class DependencyInjection
    {
        private static readonly bool IsDevelopment = EnvironmentExtension.IsDevelopment;
        private static readonly bool IsStaging = EnvironmentExtension.IsStaging;
        private static readonly bool IsProduction = EnvironmentExtension.IsPreProduction;
        private static readonly bool IsPreProduction = EnvironmentExtension.IsPreProduction;

        private static readonly string[] DefaultExposeHeaders =
        {
            "client-version", "server-version", "Upload-Offset", "Location", "Upload-Length", "Tus-Version",
            "Tus-Resumable", "Tus-Max-FileSize", "Tus-Extension", "Upload-Metadata", "Upload-Defer-Length", "Upload-Concat",
            "Location", "Upload-Offset", "Upload-Length", "client-version", "server-version"
        };

        public static IServiceCollection ConfigureDependencyInjections(this IServiceCollection services,
            IConfiguration configuration, DependencyInjectionOptions options)
        {
            services.AddMemoryCache();
            services
                .ConfigureIdentity(configuration)
                .ConfigureAuthentication(configuration)
                .ConfigureAuthorization()
                .ConfigureDatabase(configuration)
                .AddHttpClient()
                .AddRepositories()
                .ConfigureAutoMapper(options.AutoMapperAssemblies)
                .ConfigureServices()
                .ConfigureCors()
                .ConfigureRequestBodyLimit()
                //.AddSwagger(options.SwaggerAssembly)
                .AddMediator();
            return services;
        }

        public static IApplicationBuilder ConfigureLoggingMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<LoggingMiddleware>();
                
            return builder;
        }

        public static IServiceCollection ConfigureRequestBodyLimit(this IServiceCollection services)
        {
            const int maxBodyLimit = 5000000;//5mb
            const int maxFileSizeLimit = 40000000; //40mb
            const int maxLimit = maxBodyLimit + maxFileSizeLimit;
            const long multipartBodyLengthLimit = maxLimit;
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = multipartBodyLengthLimit;

            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = multipartBodyLengthLimit; // if don't set default value is: 30 MB  41MB
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = maxLimit;
                x.MultipartBodyLengthLimit = multipartBodyLengthLimit; // if don't set default value is: 128 MB
                x.MultipartHeadersLengthLimit = maxLimit;
            });
            return services;
        }

        public static IApplicationBuilder ConfigureAutoWrapperMiddleware(this IApplicationBuilder builder)
        {
            builder.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
            {
                //UseApiProblemDetailsException = true,
                IgnoreNullValue = false,
                ShowStatusCode = true,
                ShowIsErrorFlagForSuccessfulResponse = true,
                IgnoreWrapForOkRequests = true,
                IsDebug = IsDevelopment || IsStaging,
                IsApiOnly=false,
                EnableExceptionLogging = false,
                EnableResponseLogging = false,
                LogRequestDataOnException = false,
                ShouldLogRequestData = false,


                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                UseCustomExceptionFormat = false,
            });

            return builder;
        }

        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services
                .AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));

                });
            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, Assembly assembly)
        {
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GP.API",
                    Version = "v1",
                    Description = "GP Api"
                });
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

                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{assembly.GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //swagger.IncludeXmlComments(xmlPath);

            });
            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }


        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // i assume your service interfaces inherit from IRepositoryBase<>
            Assembly ass = typeof(IRepositoryIdentifier).GetTypeInfo().Assembly;

            // get all concrete types which implements IRepositoryIdentifier
            var allRepositories = ass.GetTypes().Where(t =>
                t.GetTypeInfo().IsClass &&
                !t.IsGenericType &&
                !t.GetTypeInfo().IsAbstract &&
                typeof(IRepositoryIdentifier).IsAssignableFrom(t));

            foreach (var type in allRepositories)
            {
                var allInterfaces = type.GetInterfaces();
                var mainInterfaces = allInterfaces.Where(t => typeof(IRepositoryIdentifier) != t && (!t.IsGenericType || t.GetGenericTypeDefinition() != typeof(IRepository<>)));
                foreach (var itype in mainInterfaces)
                {
                    if (allRepositories.Any(x => x != type && itype.IsAssignableFrom(x)))
                    {
                        throw new Exception("The " + itype.Name + " type has more than one implementations, please change your filter");
                    }
                    services.AddScoped(itype, type);
                }
            }
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                var context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
                var appDomains = context.AppDomains.ToList();

                bool IsOriginAllowed(string origin)
                {
                    Uri uri;
                    if (Uri.TryCreate(origin, UriKind.Absolute, out uri))
                    {
                        origin = uri.Host;
                    }

                    if (!string.IsNullOrEmpty(origin))
                    {
                        var isAllowed = appDomains.Any(c =>
                            origin.EndsWith(c.Domain, StringComparison.OrdinalIgnoreCase));

                        if (!isAllowed && !IsProduction)
                        {
                            //uri = new Uri(origin);
                            isAllowed = origin.Equals("localhost", StringComparison.OrdinalIgnoreCase);
                        }

                        return isAllowed;
                    }

                    return false;
                }

                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("")
                        .WithExposedHeaders(DefaultExposeHeaders)
                        .SetIsOriginAllowed(IsOriginAllowed);
                });
                //options.AddPolicy("upload_policy_for_tus", builder =>
                //{
                //    builder
                //        .AllowCredentials()
                //        .AllowAnyHeader()
                //        .AllowAnyMethod()
                //        .WithOrigins("")
                //        .WithExposedHeaders(TusExposeHeaders)
                //        .SetIsOriginAllowed(IsOriginAllowed);
                //});
            });
            return services;
        }

        private static IServiceCollection ConfigureAutoMapper(this IServiceCollection services, Assembly[] assemblies)
        {
            services.AddAutoMapper(assemblies);
            return services;
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<AuthService>();
            services.AddScoped<ExceptionService>();
            services.AddScoped<TokenService>();
            services.AddScoped<SignInService>();
            services.AddScoped<EmailService>();
            services.AddScoped<EmailTemplatingService>();
            services.AddScoped<AccessLimitService>();
            services.AddScoped<SmsConfirmationCodeService>();
            services.AddScoped<SmsSenderService>();
            services.AddScoped<TranslationService>();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            services.AddControllers(options =>
            {
                //options.Filters.Add<RecordNotFoundExceptionFilter>();
                options.Filters.Add<UnauthorizedExceptionFilter>();
                options.Filters.Add<RecordAlreadyExistsExceptionFilter>();
                options.Filters.Add<RecordInUseExceptionFilter>();
            });
            return services;
        }

        private static IServiceCollection ConfigureDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("DefaultPostgres")));
            return services;
        }

        private static IServiceCollection ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("authSettings");
            services.Configure<AuthSettings>(appSettingsSection);

            var authSettings = appSettingsSection.Get<AuthSettings>();

            services.AddIdentity<User, Role>(
                options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredUniqueChars = 0;
                    options.Lockout.AllowedForNewUsers = true;
                }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            return services;
        }

        private static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Add authentication with cookie authentication
            services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "YourAppName.Cookie";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30); 
                options.SlidingExpiration = true;

                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout"; 
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
            return services;
        }

        private static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                var context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
                var categories = context.PermissionCategoryPermissions
                    .Include(c => c.Permission)
                    .Include(c => c.Category);


                foreach (var permissionCategory in categories)
                {
                    //Usage : user_add
                    options.AddPolicy(
                        permissionCategory.Category.Label.ToLower() + "_" +
                        permissionCategory.Permission.Label.ToLower(),
                        policy => policy.Requirements.Add(new PermissionRequirement(
                            new PermissionRequirementModel(permissionCategory.PermissionId,
                                permissionCategory.CategoryId)
                        )));
                }

                var permissions = context.Permissions.Where(c => c.IsDirective).ToList();
                foreach (var permission in permissions)
                {
                    //Usage : admin
                    options.AddPolicy(permission.Label.ToLower(),
                        policy => policy.Requirements.Add(new PermissionRequirement(
                            new PermissionRequirementModel(permission.Label)
                        )));
                }
            });
            return services;
        }
    }
}
