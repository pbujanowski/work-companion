using Microsoft.Extensions.Configuration;

namespace WorkCompanion.Common.Configuration.Extensions;

/// <summary>
/// Extension methods for configuring application configuration providers
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Adds environment variables as a configuration source
    /// </summary>
    /// <param name="configuration">The configuration manager</param>
    /// <returns>The configuration manager for method chaining</returns>
    public static ConfigurationManager ConfigureConfigurationProviders(
        this ConfigurationManager configuration
    )
    {
        configuration.AddEnvironmentVariables();

        return configuration;
    }
}
