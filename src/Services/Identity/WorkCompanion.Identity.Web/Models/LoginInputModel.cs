using System.ComponentModel.DataAnnotations;

namespace WorkCompanion.Identity.Web.Models;

/// <summary>
/// Login input model containing email and password credentials
/// </summary>
public class LoginInputModel
{
    /// <summary>
    /// Gets or initializes the user email address
    /// </summary>
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public string? Email { get; init; }

    /// <summary>
    /// Gets or initializes the user password
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; init; }

    /// <summary>
    /// Gets or initializes a value indicating whether to remember the user login
    /// </summary>
    [Display(Name = "Remember me")]
    public bool RememberMe { get; init; }
}
