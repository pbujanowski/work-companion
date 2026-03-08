namespace WorkCompanion.Identity.Web.Models;

/// <summary>
/// Registration view model containing registration input data and return URL
/// </summary>
public class RegisterViewModel
{
    /// <summary>
    /// Gets or sets the registration input data
    /// </summary>
    public RegisterInputModel Input { get; set; } = new();

    /// <summary>
    /// Gets or sets the URL to return to after successful registration
    /// </summary>
    public string? ReturnUrl { get; set; }
}
