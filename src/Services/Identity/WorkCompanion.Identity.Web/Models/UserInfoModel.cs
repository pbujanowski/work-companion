using System.Text.Json.Serialization;

namespace WorkCompanion.Identity.Web.Models;

/// <summary>
/// User information model for OAuth/OpenID Connect userinfo endpoint
/// </summary>
public class UserInfoModel
{
    /// <summary>
    /// Gets or sets the user's unique subject identifier
    /// </summary>
    [JsonPropertyName("sub")]
    public string? Subject { get; set; }

    /// <summary>
    /// Gets or sets the user's email address
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the user's full name
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the user's assigned roles
    /// </summary>
    [JsonPropertyName("role")]
    public IEnumerable<string>? Role { get; set; }
}
