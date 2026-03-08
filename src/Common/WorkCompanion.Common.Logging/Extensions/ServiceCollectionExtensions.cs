using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace WorkCompanion.Common.Logging.Extensions;

/// <summary>
/// Extension methods for configuring logging services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures Serilog logging for the application
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The application configuration</param>
    /// <returns>The service collection for method chaining</returns>
    public static IServiceCollection ConfigureLogging(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        LoggingInitializer.Initialize(configuration);
        services.AddSerilog(dispose: true);

        return services;
    }
}
