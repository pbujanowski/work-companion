using Microsoft.AspNetCore.Identity;

namespace WorkCompanion.Identity.Domain.Entities;

/// <summary>
/// Represents an application role for role-based access control
/// </summary>
public class ApplicationRole : IdentityRole<Guid>
{
    /// <summary>
    /// Initializes a new instance of the ApplicationRole class
    /// </summary>
    public ApplicationRole() { }

    /// <summary>
    /// Initializes a new instance of the ApplicationRole class with a name
    /// </summary>
    /// <param name="name">The name of the role</param>
    public ApplicationRole(string name)
    {
        Name = name;
    }
}
