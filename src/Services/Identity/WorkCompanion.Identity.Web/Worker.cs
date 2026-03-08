using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using WorkCompanion.Common.Configuration.Exceptions;
using WorkCompanion.Identity.Application.Constants;
using WorkCompanion.Identity.Domain.Entities;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace WorkCompanion.Identity.Web;

/// <summary>
/// Hosted service that initializes OAuth applications, roles, and seed users on startup
/// </summary>
public class Worker(IServiceProvider serviceProvider, IConfiguration configuration) : IHostedService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<Worker> _logger = serviceProvider.GetRequiredService<ILogger<Worker>>();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Worker service initialization");

        await using var scope = _serviceProvider.CreateAsyncScope();

        var applicationManager =
            scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await InitializeMobileApplicationAsync(applicationManager, cancellationToken);
        await InitializeRolesAsync(roleManager, cancellationToken);
        await InitializeUsersAsync(userManager, cancellationToken);

        _logger.LogInformation("Worker service initialization completed successfully");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task InitializeMobileApplicationAsync(
        IOpenIddictApplicationManager applicationManager,
        CancellationToken cancellationToken)
    {
        var mobileAppConfig = _configuration
            .GetSection("OpenIddict:MobileApp")
            .Get<MobileAppConfiguration>()
            ?? throw new ConfigurationException("OpenIddict:MobileApp");

        if (await applicationManager.FindByClientIdAsync(mobileAppConfig.ClientId, cancellationToken) is not null)
        {
            _logger.LogInformation("Mobile application '{ClientId}' already exists", mobileAppConfig.ClientId);
            return;
        }

        try
        {
            await applicationManager.CreateAsync(
                new OpenIddictApplicationDescriptor
                {
                    ClientId = mobileAppConfig.ClientId,
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = mobileAppConfig.DisplayName,
                    ClientType = ClientTypes.Public,
                    PostLogoutRedirectUris = { new Uri(mobileAppConfig.PostLogoutRedirectUri) },
                    RedirectUris = { new Uri(mobileAppConfig.RedirectUri) },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + AppScopes.Services.Posts,
                    },
                    Requirements = { Requirements.Features.ProofKeyForCodeExchange },
                },
                cancellationToken
            );

            _logger.LogInformation("Mobile application '{ClientId}' created successfully", mobileAppConfig.ClientId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create mobile application '{ClientId}'", mobileAppConfig.ClientId);
            throw;
        }
    }

    private async Task InitializeRolesAsync(
        RoleManager<ApplicationRole> roleManager,
        CancellationToken cancellationToken)
    {
        var roles = new[] { AppRoles.Administrator, AppRoles.User };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                try
                {
                    await roleManager.CreateAsync(new ApplicationRole(roleName));
                    _logger.LogInformation("Role '{RoleName}' created successfully", roleName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create role '{RoleName}'", roleName);
                    throw;
                }
            }
            else
            {
                _logger.LogInformation("Role '{RoleName}' already exists", roleName);
            }
        }
    }

    private async Task InitializeUsersAsync(
        UserManager<ApplicationUser> userManager,
        CancellationToken cancellationToken)
    {
        // Initialize Administrator
        var administratorEmail = _configuration
            .GetSection("Administrator:Email")
            .Get<string>();

        var administratorPassword = _configuration
            .GetSection("Administrator:Password")
            .Get<string>();

        if (string.IsNullOrEmpty(administratorEmail) || string.IsNullOrEmpty(administratorPassword))
        {
            _logger.LogWarning("Administrator credentials not configured. Please set Administrator:Email and Administrator:Password using: dotnet user-secrets set \"Administrator:Email\" \"<email>\" && dotnet user-secrets set \"Administrator:Password\" \"<password>\"");
            return;
        }

        ValidateAdminPassword(administratorPassword);

        if (await userManager.FindByEmailAsync(administratorEmail) is null)
        {
            await CreateUserAsync(userManager, administratorEmail, administratorPassword, AppRoles.Administrator);
        }
        else
        {
            _logger.LogInformation("Administrator user '{Email}' already exists", administratorEmail);
        }

        // Initialize Regular User
        var userEmail = _configuration
            .GetSection("User:Email")
            .Get<string>();

        var userPassword = _configuration
            .GetSection("User:Password")
            .Get<string>();

        if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userPassword))
        {
            _logger.LogWarning("Regular user credentials not configured. Please set User:Email and User:Password using: dotnet user-secrets set \"User:Email\" \"<email>\" && dotnet user-secrets set \"User:Password\" \"<password>\"");
            return;
        }

        if (await userManager.FindByEmailAsync(userEmail) is null)
        {
            await CreateUserAsync(userManager, userEmail, userPassword, AppRoles.User);
        }
        else
        {
            _logger.LogInformation("User '{Email}' already exists", userEmail);
        }
    }

    private async Task CreateUserAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string roleName)
    {
        try
        {
            var user = new ApplicationUser { Email = email, UserName = email };
            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User creation failed: {errors}");
            }

            var createdUser = await userManager.FindByEmailAsync(email)
                ?? throw new InvalidOperationException($"User '{email}' creation verification failed");

            await userManager.AddToRoleAsync(createdUser, roleName);
            _logger.LogInformation("User '{Email}' created successfully with role '{RoleName}'", email, roleName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user '{Email}' with role '{RoleName}'", email, roleName);
            throw;
        }
    }

    private void ValidateAdminPassword(string password)
    {
        // Admin passwords should be stronger than regular user passwords
        // Minimum 12 characters, must contain uppercase, lowercase, digit, and special character
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(password) || password.Length < 12)
            errors.Add("Password must be at least 12 characters long");

        if (!Regex.IsMatch(password, @"[A-Z]"))
            errors.Add("Password must contain at least one uppercase letter");

        if (!Regex.IsMatch(password, @"[a-z]"))
            errors.Add("Password must contain at least one lowercase letter");

        if (!Regex.IsMatch(password, @"\d"))
            errors.Add("Password must contain at least one digit");

        if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':"",.<>?/\\|`~]"))
            errors.Add("Password must contain at least one special character");

        if (errors.Count > 0)
        {
            var errorMessage = $"Administrator password does not meet security requirements: {string.Join("; ", errors)}";
            _logger.LogError(errorMessage);
            throw new ConfigurationException(errorMessage);
        }
    }
}

/// <summary>
/// Configuration for mobile application OAuth settings
/// </summary>
public class MobileAppConfiguration
{
    /// <summary>
    /// Gets or sets the OAuth client ID for the mobile application
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name for the mobile application
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the OAuth redirect URI for the mobile application
    /// </summary>
    public string RedirectUri { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the post-logout redirect URI for the mobile application
    /// </summary>
    public string PostLogoutRedirectUri { get; set; } = string.Empty;
}
