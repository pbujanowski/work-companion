using OpenIddict.EntityFrameworkCore.Models;

namespace WorkCompanion.Identity.Infrastructure.Data;

/// <summary>
/// Represents an OAuth 2.0 / OpenID Connect authorization
/// </summary>
public class ApplicationAuthorization
    : OpenIddictEntityFrameworkCoreAuthorization<Guid, ApplicationClient, ApplicationToken>;
