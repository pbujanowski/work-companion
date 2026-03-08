using FluentValidation;
using WorkCompanion.Identity.Application.Dtos;

namespace WorkCompanion.Identity.Application.Validators;

/// <summary>
/// Validator for ApplicationUserDto
/// </summary>
public class ApplicationUserDtoValidator : AbstractValidator<ApplicationUserDto>
{
    /// <summary>
    /// Initializes a new instance of the ApplicationUserDtoValidator class with validation rules
    /// </summary>
    public ApplicationUserDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MinimumLength(3)
            .WithMessage("Username must be at least 3 characters long.");
    }
}
