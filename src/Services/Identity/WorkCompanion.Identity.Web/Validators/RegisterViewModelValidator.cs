using FluentValidation;
using WorkCompanion.Identity.Web.Models;

namespace WorkCompanion.Identity.Web.Validators;

/// <summary>
/// Validator for user registration view model
/// </summary>
public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
{
    /// <summary>
    /// Initializes a new instance of the RegisterViewModelValidator class with validation rules
    /// </summary>
    /// <param name="inputValidator">The register input model validator</param>
    public RegisterViewModelValidator(IValidator<RegisterInputModel> inputValidator)
    {
        RuleFor(x => x.Input)
            .NotNull()
            .WithMessage("Registration input is required.")
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
