namespace WorkCompanion.Common.Configuration.Configurations;

/// <summary>
/// Configuration settings for OpenID Connect (OIDC) authentication
/// </summary>
public class OidcConfiguration
{
    /// <summary>
    /// Gets or sets the OIDC token issuer URL
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// Gets or sets the OIDC audience identifier
    /// </summary>
    public string? Audience { get; set; }

    /// <summary>
    /// Gets or sets the OAuth client ID
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Gets or sets the OAuth client secret
    /// </summary>
    public string? ClientSecret { get; set; }
}
