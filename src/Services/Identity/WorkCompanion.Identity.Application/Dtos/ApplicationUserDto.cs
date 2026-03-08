namespace WorkCompanion.Identity.Application.Dtos;

/// <summary>
/// Data transfer object for application user information
/// </summary>
/// <param name="Id">The unique identifier of the user</param>
/// <param name="Email">The email address of the user</param>
/// <param name="UserName">The username of the user</param>
public record ApplicationUserDto(Guid? Id, string? Email, string? UserName);
