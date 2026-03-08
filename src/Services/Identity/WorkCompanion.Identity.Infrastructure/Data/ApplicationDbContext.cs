using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkCompanion.Identity.Domain.Entities;

namespace WorkCompanion.Identity.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for the identity service
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.UseOpenIddict<
            ApplicationClient,
            ApplicationAuthorization,
            ApplicationScope,
            ApplicationToken,
            Guid
        >();
    }
}
