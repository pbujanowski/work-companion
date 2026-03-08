using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkCompanion.Identity.Application.Constants;
using WorkCompanion.Identity.Domain.Entities;
using WorkCompanion.Identity.Web.Logging;
using WorkCompanion.Identity.Web.Models;

namespace WorkCompanion.Identity.Web.Controllers;

/// <summary>
/// Handles user account operations including login, registration, and logout
/// </summary>
public class AccountController(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    ILogger<AccountController> logger
) : Controller
{
    private readonly ILogger<AccountController> _logger = logger;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    /// <summary>
    /// Displays the user login page
    /// </summary>
    /// <param name="returnUrl">The URL to redirect to after successful login</param>
    /// <returns>The login view</returns>
    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        return View(new LoginViewModel { Input = new LoginInputModel(), ReturnUrl = returnUrl });
    }

    /// <summary>
    /// Processes user login request
    /// </summary>
    /// <param name="model">The login credentials and return URL</param>
    /// <param name="returnUrl">The URL to redirect to after successful login</param>
    /// <returns>Redirects to return URL on success, or redisplays the login form with errors</returns>
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Input.Email!,
                model.Input.Password!,
                model.Input.RememberMe,
                false
            );

            if (result.Succeeded)
            {
                _logger.LogInformation(LoggerEventIds.UserLogin, "User logged in.");
                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        return View(model);
    }

    /// <summary>
    /// Displays the user registration page
    /// </summary>
    /// <param name="returnUrl">The URL to redirect to after successful registration</param>
    /// <returns>The registration view</returns>
    [HttpGet]
    public IActionResult Register(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        return View(new RegisterViewModel { ReturnUrl = returnUrl });
    }

    /// <summary>
    /// Processes user registration request
    /// </summary>
    /// <param name="model">The registration form data</param>
    /// <param name="returnUrl">The URL to redirect to after successful registration</param>
    /// <returns>Redirects to return URL on success, or redisplays the registration form with errors</returns>
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = model.Input.Email!,
                Email = model.Input.Email!,
            };

            var userCreatedResult = await _userManager.CreateAsync(user, model.Input.Password!);

            if (userCreatedResult.Succeeded)
            {
                _logger.LogInformation(
                    LoggerEventIds.UserCreated,
                    "User created a new account with password."
                );

                await _userManager.AddToRoleAsync(user, AppRoles.User);
                await _signInManager.SignInAsync(user, false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in userCreatedResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }
}
