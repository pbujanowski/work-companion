using OpenIddict.EntityFrameworkCore.Models;

namespace WorkCompanion.Identity.Infrastructure.Data;

/// <summary>
/// Represents an OAuth 2.0 / OpenID Connect scope
/// </summary>
public class ApplicationScope : OpenIddictEntityFrameworkCoreScope<Guid>;
