using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WorkCompanion.Identity.Web.Models;

/// <summary>
/// Logout view model containing logout request data
/// </summary>
public class LogoutViewModel
{
    /// <summary>
    /// Gets or sets the logout request ID
    /// </summary>
    /// <remarks>
    /// This property is never bound from the HTTP request body (marked with BindNever attribute)
    /// </remarks>
    [BindNever]
    public string? RequestId { get; set; }
}
