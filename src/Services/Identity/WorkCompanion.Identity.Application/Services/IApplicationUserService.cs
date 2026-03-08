using WorkCompanion.Identity.Application.Dtos;

namespace WorkCompanion.Identity.Application.Services;

/// <summary>
/// Service interface for retrieving application user information
/// </summary>
public interface IApplicationUserService
{
    /// <summary>
    /// Gets a user by their unique identifier asynchronously
    /// </summary>
    /// <param name="id">The unique identifier of the user</param>
    /// <returns>The user DTO if found; otherwise null</returns>
    Task<ApplicationUserDto?> GetUserByIdAsync(Guid id);
}
