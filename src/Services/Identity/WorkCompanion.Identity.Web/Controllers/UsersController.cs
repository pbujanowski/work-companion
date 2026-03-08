using Microsoft.AspNetCore.Mvc;
using WorkCompanion.Identity.Application.Services;

namespace WorkCompanion.Identity.Web.Controllers;

/// <summary>
/// API endpoints for user management
/// </summary>
[ApiController]
[Route("[controller]")]
public class UsersController(
    IApplicationUserService applicationUserService,
    ILogger<UsersController> logger
) : ControllerBase
{
    private readonly IApplicationUserService _applicationUserService = applicationUserService;
    private readonly ILogger<UsersController> _logger = logger;

    /// <summary>
    /// Gets a user by their ID
    /// </summary>
    /// <param name="userId">The unique identifier of the user</param>
    /// <param name="displayName">If true, returns only the email; otherwise returns full user object</param>
    /// <returns>The requested user</returns>
    /// <response code="200">User found</response>
    /// <response code="400">Invalid user ID format</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(
        [FromRoute] Guid userId,
        [FromQuery] bool displayName = false)
    {
        if (userId == Guid.Empty)
        {
            _logger.LogWarning("Invalid user ID: empty GUID provided");
            return BadRequest(new { message = "Invalid user ID format" });
        }

        try
        {
            var user = await _applicationUserService.GetUserByIdAsync(userId);
            if (user is null)
            {
                _logger.LogInformation("User not found. UserId: {UserId}", userId);
                return NotFound(new { message = "User not found" });
            }

            if (displayName)
            {
                return Ok(new { email = user.Email });
            }

            return Ok(user);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation cancelled while retrieving user {UserId}", userId);
            return StatusCode(StatusCodes.Status408RequestTimeout,
                new { message = "Request timeout while retrieving user" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid operation while retrieving user {UserId}", userId);
            return StatusCode(StatusCodes.Status400BadRequest,
                new { message = "Invalid operation" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving user {UserId}", userId);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred processing your request" });
        }
    }
}
