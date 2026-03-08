namespace WorkCompanion.Identity.Web.Models;

/// <summary>
/// Error view model for displaying error information
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Gets or sets the request ID associated with the error
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether the RequestId should be displayed
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
