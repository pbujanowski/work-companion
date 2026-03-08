using OpenIddict.EntityFrameworkCore.Models;

namespace WorkCompanion.Identity.Infrastructure.Data;

/// <summary>
/// Represents an OAuth 2.0 / OpenID Connect token
/// </summary>
public class ApplicationToken
    : OpenIddictEntityFrameworkCoreToken<Guid, ApplicationClient, ApplicationAuthorization>;
