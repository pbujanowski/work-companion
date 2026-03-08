using FluentValidation;
using WorkCompanion.Identity.Web.Models;

namespace WorkCompanion.Identity.Web.Validators;

/// <summary>
/// Validator for logout view model
/// </summary>
public class LogoutViewModelValidator : AbstractValidator<LogoutViewModel>
{
    /// <summary>
    /// Initializes a new instance of the LogoutViewModelValidator class with validation rules
    /// </summary>
    public LogoutViewModelValidator()
    {
        RuleFor(x => x.RequestId)
            .Must(id => string.IsNullOrEmpty(id) || id.Length <= 100)
            .WithMessage("Request ID must not exceed 100 characters.");
    }
}
