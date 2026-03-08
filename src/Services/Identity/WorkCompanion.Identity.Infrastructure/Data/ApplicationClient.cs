using OpenIddict.EntityFrameworkCore.Models;

namespace WorkCompanion.Identity.Infrastructure.Data;

/// <summary>
/// Represents an OAuth 2.0 / OpenID Connect client application
/// </summary>
public class ApplicationClient
    : OpenIddictEntityFrameworkCoreApplication<Guid, ApplicationAuthorization, ApplicationToken>;
