using FluentValidation;
using WorkCompanion.Identity.Web.Models;

namespace WorkCompanion.Identity.Web.Validators;

/// <summary>
/// Validator for authorization view model
/// </summary>
public class AuthorizeViewModelValidator : AbstractValidator<AuthorizeViewModel>
{
    /// <summary>
    /// Initializes a new instance of the AuthorizeViewModelValidator class with validation rules
    /// </summary>
    public AuthorizeViewModelValidator()
    {
        RuleFor(x => x.ApplicationName)
            .NotEmpty()
            .WithMessage("Application name is required.");

        RuleFor(x => x.Scope)
            .NotEmpty()
            .WithMessage("Scope is required.");
    }
}
