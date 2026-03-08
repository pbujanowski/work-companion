using FluentValidation;
using WorkCompanion.Identity.Web.Models;

namespace WorkCompanion.Identity.Web.Validators;

/// <summary>
/// Validator for login view model
/// </summary>
public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
{
    /// <summary>
    /// Initializes a new instance of the LoginViewModelValidator class with validation rules
    /// </summary>
    /// <param name="inputValidator">The login input model validator</param>
    public LoginViewModelValidator(IValidator<LoginInputModel> inputValidator)
    {
        RuleFor(x => x.Input)
            .NotNull()
            .WithMessage("Login input is required.")
            .SetValidator(inputValidator);

        RuleFor(x => x.ReturnUrl)
            .Must(url => string.IsNullOrEmpty(url) || IsValidReturnUrl(url))
            .WithMessage("Return URL is invalid.");
    }

    private static bool IsValidReturnUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return true;

        // Validate return URL is relative (security: prevent open redirect)
        return url.StartsWith("/") && !url.StartsWith("//");
    }
}
