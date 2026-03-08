using Xunit;
using WorkCompanion.Identity.Web.Models;
using WorkCompanion.Identity.Web.Validators;

namespace WorkCompanion.Identity.Web.Tests;

public class LoginInputModelTests
{
    [Fact]
    public void LoginInputModel_CanBeCreatedWithValidData()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var rememberMe = true;

        // Act
        var model = new LoginInputModel
        {
            Email = email,
            Password = password,
            RememberMe = rememberMe
        };

        // Assert
        Assert.Equal(email, model.Email);
        Assert.Equal(password, model.Password);
        Assert.True(model.RememberMe);
    }

    [Fact]
    public void LoginInputModel_CanHaveNullEmail()
    {
        // Act
        var model = new LoginInputModel();

        // Assert
        Assert.Null(model.Email);
        Assert.Null(model.Password);
    }

    [Fact]
    public void LoginInputModel_RememberMeDefaultsFalse()
    {
        // Act
        var model = new LoginInputModel();

        // Assert
        Assert.False(model.RememberMe);
    }
}

public class LoginInputModelValidatorTests
{
    private readonly LoginInputModelValidator _validator = new();

    [Fact]
    public void Validator_IsValidWithCorrectData()
    {
        // Arrange
        var model = new LoginInputModel
        {
            Email = "test@example.com",
            Password = "password123"
        };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_FailsWithNullEmail()
    {
        // Arrange
        var model = new LoginInputModel
        {
            Email = null,
            Password = "password123"
        };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validator_FailsWithEmptyEmail()
    {
        // Arrange
        var model = new LoginInputModel
        {
            Email = "",
            Password = "password123"
        };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_FailsWithInvalidEmailFormat()
    {
        // Arrange
        var model = new LoginInputModel
        {
            Email = "invalid-email",
            Password = "password123"
        };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_FailsWithNullPassword()
    {
        // Arrange
        var model = new LoginInputModel
        {
            Email = "test@example.com",
            Password = null
        };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }

    [Fact]
    public void Validator_FailsWithEmptyPassword()
    {
        // Arrange
        var model = new LoginInputModel
        {
            Email = "test@example.com",
            Password = ""
        };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_SucceedsWithValidEmailFormats()
    {
        // Arrange & Act & Assert
        var validEmails = new[]
        {
            "user@example.com",
            "user.name@example.co.uk",
            "user+tag@example.com",
            "123@example.com"
        };

        foreach (var email in validEmails)
        {
            var model = new LoginInputModel { Email = email, Password = "password123" };
            var result = _validator.Validate(model);
            Assert.True(result.IsValid, $"Email {email} should be valid");
        }
    }
}

public class RegisterInputModelTests
{
    [Fact]
    public void RegisterInputModel_CanBeCreatedWithValidData()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var confirmPassword = "password123";

        // Act
        var model = new RegisterInputModel
        {
            Email = email,
            Password = password,
            ConfirmPassword = confirmPassword
        };

        // Assert
        Assert.Equal(email, model.Email);
        Assert.Equal(password, model.Password);
        Assert.Equal(confirmPassword, model.ConfirmPassword);
    }
}

public class RegisterInputModelValidatorTests
{
    private readonly RegisterInputModelValidator _validator = new();

    [Fact]
    public void Validator_IsValidWithMatchingPasswords()
    {
        // Arrange
        var model = new RegisterInputModel
        {
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_FailsWithNullEmail()
    {
        // Arrange
        var model = new RegisterInputModel { Email = null, Password = "Pass123!", ConfirmPassword = "Pass123!" };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_FailsWithInvalidEmail()
    {
        // Arrange
        var model = new RegisterInputModel { Email = "invalid", Password = "Pass123!", ConfirmPassword = "Pass123!" };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_FailsWithNullPassword()
    {
        // Arrange
        var model = new RegisterInputModel { Email = "test@example.com", Password = null, ConfirmPassword = "Pass123!" };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_FailsWithShortPassword()
    {
        // Arrange
        var model = new RegisterInputModel { Email = "test@example.com", Password = "Pass1", ConfirmPassword = "Pass1" };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_FailsWithNonMatchingPasswords()
    {
        // Arrange
        var model = new RegisterInputModel
        {
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "DifferentPassword!"
        };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
    }
}
