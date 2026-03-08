namespace WorkCompanion.Common.Configuration.Configurations;

/// <summary>
/// Configuration settings for Cross-Origin Resource Sharing (CORS)
/// </summary>
public class CorsConfiguration
{
    /// <summary>
    /// Gets or sets the allowed CORS origins (domain URLs)
    /// </summary>
    public string[]? Origins { get; set; }
}
