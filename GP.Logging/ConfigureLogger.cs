using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Sinks.RollingFileAlternate;

namespace GP.Logging
{
    public static class ConfigureLogger
    {
        public static LoggerConfiguration BuildLoggerConfiguration(this LoggerConfiguration loggerConfiguration, IServiceCollection services, IConfiguration configuration)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = env == Environments.Development;
            var projectName = Assembly.GetExecutingAssembly().GetName().Name;
            //var logPath = isDevelopment ? $"../{projectName}/Logs" : "Logs";
            var logPath = $"../{projectName}/Logs";


            var conf = loggerConfiguration.MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(LogEventLevel.Information); //300mb


            conf = conf.WriteTo
                .RollingFileAlternate(new RenderedCompactJsonFormatter(), logPath, fileSizeLimitBytes: 314572800);

            return conf;
        }
    }
}