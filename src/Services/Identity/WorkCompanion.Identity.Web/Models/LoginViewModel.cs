namespace WorkCompanion.Identity.Web.Models;

/// <summary>
/// Login view model containing login input data and return URL
/// </summary>
public class LoginViewModel
{
    /// <summary>
    /// Gets or sets the login input data
    /// </summary>
    public LoginInputModel Input { get; set; } = new();

    /// <summary>
    /// Gets or sets the URL to return to after successful login
    /// </summary>
    public string? ReturnUrl { get; set; }
}
