using System.ComponentModel.DataAnnotations;

namespace WorkCompanion.Identity.Web.Models;

/// <summary>
/// Authorization view model containing OAuth application and scope information
/// </summary>
public class AuthorizeViewModel
{
    /// <summary>
    /// Gets or sets the name of the OAuth application requesting authorization
    /// </summary>
    [Display(Name = "Application")]
    public string? ApplicationName { get; set; }

    /// <summary>
    /// Gets or sets the OAuth scopes being requested
    /// </summary>
    [Display(Name = "Scope")]
    public string? Scope { get; set; }
}
