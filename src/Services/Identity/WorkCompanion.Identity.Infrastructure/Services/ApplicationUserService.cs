using Microsoft.EntityFrameworkCore;
using WorkCompanion.Identity.Application.Dtos;
using WorkCompanion.Identity.Application.Services;
using WorkCompanion.Identity.Infrastructure.Data;

namespace WorkCompanion.Identity.Infrastructure.Services;

/// <summary>
/// Service for retrieving application user information
/// </summary>
public class ApplicationUserService(ApplicationDbContext dbContext) : IApplicationUserService
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    /// <summary>
    /// Gets a user by their ID asynchronously
    /// </summary>
    /// <param name="id">The unique identifier of the user</param>
    /// <returns>The user DTO if found; otherwise null</returns>
    public async Task<ApplicationUserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
        {
            return null;
        }

        return new ApplicationUserDto(user.Id, user.Email, user.UserName);
    }
}
