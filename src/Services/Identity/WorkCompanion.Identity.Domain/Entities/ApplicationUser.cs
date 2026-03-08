using Microsoft.AspNetCore.Identity;

namespace WorkCompanion.Identity.Domain.Entities;

/// <summary>
/// Represents an application user with identity information
/// </summary>
public class ApplicationUser : IdentityUser<Guid>;
