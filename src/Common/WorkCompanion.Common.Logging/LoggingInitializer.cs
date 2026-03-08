using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace WorkCompanion.Common.Logging;

/// <summary>
/// Initializes Serilog logger with environment-specific configuration
/// </summary>
internal static class LoggingInitializer
{
    public static void Initialize(IConfiguration configuration)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "WorkCompanion.Identity")
            .Enrich.WithProperty("Environment", env);

        if (env == "Development")
        {
            loggerConfiguration
                .WriteTo.Console(new CompactJsonFormatter());
        }
        else
        {
            // Ensure logs directory exists
            var logsPath = Path.Combine(AppContext.BaseDirectory, "logs");
            Directory.CreateDirectory(logsPath);

            loggerConfiguration
                .WriteTo.Console()
                .WriteTo.File(
                    Path.Combine(logsPath, "workcompanion-.txt"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}");
        }

        Log.Logger = loggerConfiguration.CreateLogger();
    }
}
