using System.ComponentModel.DataAnnotations;

namespace WorkCompanion.Identity.Web.Models;

/// <summary>
/// Registration input model containing user registration credentials
/// </summary>
public class RegisterInputModel
{
    /// <summary>
    /// Gets or initializes the user email address
    /// </summary>
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; init; }

    /// <summary>
    /// Gets or initializes the user password
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; init; }

    /// <summary>
    /// Gets or initializes the password confirmation value
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    public string? ConfirmPassword { get; init; }
}
