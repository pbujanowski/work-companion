using FluentValidation;
using WorkCompanion.Identity.Web.Models;

namespace WorkCompanion.Identity.Web.Validators;

/// <summary>
/// Validator for login input model
/// </summary>
public class LoginInputModelValidator : AbstractValidator<LoginInputModel>
{
    /// <summary>
    /// Initializes a new instance of the LoginInputModelValidator class with validation rules
    /// </summary>
    public LoginInputModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}
