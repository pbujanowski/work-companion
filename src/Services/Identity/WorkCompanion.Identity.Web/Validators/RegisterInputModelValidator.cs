using FluentValidation;
using WorkCompanion.Identity.Web.Models;

namespace WorkCompanion.Identity.Web.Validators;

/// <summary>
/// Validator for user registration input model
/// </summary>
public class RegisterInputModelValidator : AbstractValidator<RegisterInputModel>
{
    /// <summary>
    /// Initializes a new instance of the RegisterInputModelValidator class with validation rules
    /// </summary>
    public RegisterInputModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100)
            .WithMessage("Password must not exceed 100 characters.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Password confirmation is required.")
            .Equal(x => x.Password)
            .WithMessage("Password and confirmation password do not match.");
    }
}
