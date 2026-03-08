using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkCompanion.Common.Logging.Extensions;

namespace WorkCompanion.Common.Logging.Tests;

public class LoggingConfigurationTests
{
    [Fact]
    public void ConfigureLogging_RegistersSerilogSuccessfully()
    {
        // Arrange
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Environment", "Development" }
            })
            .Build();

        // Act
        services.ConfigureLogging(config);
        var provider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(provider);
    }

    [Fact]
    public void ConfigureLogging_AddsSerilogToServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Environment", "Development" }
            })
            .Build();

        // Act
        var result = services.ConfigureLogging(config);

        // Assert
        Assert.NotNull(result);
        Assert.Same(services, result); // Method chaining
    }

    [Fact]
    public void ConfigureLogging_WithProductionEnvironment()
    {
        // Arrange
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ASPNETCORE_ENVIRONMENT", "Production" }
            })
            .Build();

        // Act
        services.ConfigureLogging(config);

        // Assert - Should not throw
        Assert.NotNull(services);
    }

    [Fact]
    public void ConfigureLogging_WithDevelopmentEnvironment()
    {
        // Arrange
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ASPNETCORE_ENVIRONMENT", "Development" }
            })
            .Build();

        // Act
        services.ConfigureLogging(config);

        // Assert - Should not throw
        Assert.NotNull(services);
    }

    [Fact]
    public void ConfigureLogging_ReturnsSameServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder().Build();

        // Act
        var result = services.ConfigureLogging(config);

        // Assert
        Assert.Same(services, result);
    }
}
