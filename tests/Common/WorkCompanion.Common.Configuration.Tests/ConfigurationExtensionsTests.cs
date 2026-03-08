using Xunit;
using Microsoft.Extensions.Configuration;
using WorkCompanion.Common.Configuration.Extensions;
using WorkCompanion.Common.Configuration.Configurations;
using WorkCompanion.Common.Configuration.Exceptions;

namespace WorkCompanion.Common.Configuration.Tests;

public class ConfigurationExtensionsTests
{
    [Fact]
    public void ConfigureConfigurationProviders_ReturnsConfigurationManager()
    {
        // Arrange
        var config = new ConfigurationManager();

        // Act
        var result = config.ConfigureConfigurationProviders();

        // Assert
        Assert.NotNull(result);
        Assert.Same(config, result);
    }

    [Fact]
    public void ConfigureConfigurationProviders_AllowsMethodChaining()
    {
        // Arrange
        var config = new ConfigurationManager();

        // Act
        var result = config.ConfigureConfigurationProviders();

        // Assert
        Assert.NotNull(result);
        Assert.Same(config, result);
    }
}

public class CorsConfigurationTests
{
    [Fact]
    public void CorsConfiguration_CanSetAndGetOrigins()
    {
        // Arrange
        var config = new CorsConfiguration();
        var origins = new[] { "https://localhost:3000", "https://example.com" };

        // Act
        config.Origins = origins;

        // Assert
        Assert.NotNull(config.Origins);
        Assert.Equal(origins, config.Origins);
    }

    [Fact]
    public void CorsConfiguration_OriginsCanBeNull()
    {
        // Arrange
        var config = new CorsConfiguration();

        // Act & Assert
        Assert.Null(config.Origins);
    }
}

public class OidcConfigurationTests
{
    [Fact]
    public void OidcConfiguration_CanSetAndGetAllProperties()
    {
        // Arrange
        var config = new OidcConfiguration();
        var issuer = "https://auth.example.com";
        var audience = "api-service";
        var clientId = "client-123";
        var clientSecret = "secret-123";

        // Act
        config.Issuer = issuer;
        config.Audience = audience;
        config.ClientId = clientId;
        config.ClientSecret = clientSecret;

        // Assert
        Assert.Equal(issuer, config.Issuer);
        Assert.Equal(audience, config.Audience);
        Assert.Equal(clientId, config.ClientId);
        Assert.Equal(clientSecret, config.ClientSecret);
    }

    [Fact]
    public void OidcConfiguration_AllPropertiesCanBeNull()
    {
        // Arrange & Act
        var config = new OidcConfiguration();

        // Assert
        Assert.Null(config.Issuer);
        Assert.Null(config.Audience);
        Assert.Null(config.ClientId);
        Assert.Null(config.ClientSecret);
    }
}

public class ConfigurationExceptionTests
{
    [Fact]
    public void ConfigurationException_CreatedWithKey()
    {
        // Arrange
        var key = "TestConfigKey";

        // Act
        var exception = new ConfigurationException(key);

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ConfigurationException>(exception);
        Assert.Contains(key, exception.Message);
    }

    [Fact]
    public void ConfigurationException_ContainsKeyInMessage()
    {
        // Arrange
        var key = "MissingKey";

        // Act
        var exception = new ConfigurationException(key);

        // Assert
        Assert.NotNull(exception);
        Assert.Contains(key, exception.Message);
    }
}
