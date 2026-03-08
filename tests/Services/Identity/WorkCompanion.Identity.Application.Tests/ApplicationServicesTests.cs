using Xunit;
using WorkCompanion.Identity.Application.Constants;
using WorkCompanion.Identity.Application.Dtos;
using WorkCompanion.Identity.Application.Validators;

namespace WorkCompanion.Identity.Application.Tests;

public class ApplicationUserDtoTests
{
    [Fact]
    public void ApplicationUserDto_CanBeCreatedWithAllValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var email = "test@example.com";
        var userName = "testuser";

        // Act
        var dto = new ApplicationUserDto(id, email, userName);

        // Assert
        Assert.Equal(id, dto.Id);
        Assert.Equal(email, dto.Email);
        Assert.Equal(userName, dto.UserName);
    }

    [Fact]
    public void ApplicationUserDto_CanBeCreatedWithNullValues()
    {
        // Act
        var dto = new ApplicationUserDto(null, null, null);

        // Assert
        Assert.Null(dto.Id);
        Assert.Null(dto.Email);
        Assert.Null(dto.UserName);
    }

    [Fact]
    public void ApplicationUserDto_IsRecord()
    {
        // Arrange
        var id = Guid.NewGuid();
        var email = "test@example.com";
        var userName = "testuser";

        // Act
        var dto1 = new ApplicationUserDto(id, email, userName);
        var dto2 = new ApplicationUserDto(id, email, userName);

        // Assert - Records support value equality
        Assert.Equal(dto1, dto2);
    }
}

public class ApplicationUserDtoValidatorTests
{
    private readonly ApplicationUserDtoValidator _validator = new();

    [Fact]
    public void Validator_IsValidWithCorrectData()
    {
        // Arrange
        var dto = new ApplicationUserDto(
            Guid.NewGuid(),
            "test@example.com",
            "testuser"
        );

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_FailsWithNullId()
    {
        // Arrange
        var dto = new ApplicationUserDto(null, "test@example.com", "testuser");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_SucceedsWithValidGuid()
    {
        // Arrange
        var dto = new ApplicationUserDto(Guid.NewGuid(), "test@example.com", "testuser");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_FailsWithNullEmail()
    {
        // Arrange
        var dto = new ApplicationUserDto(Guid.NewGuid(), null, "testuser");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validator_FailsWithInvalidEmail()
    {
        // Arrange
        var dto = new ApplicationUserDto(Guid.NewGuid(), "invalid-email", "testuser");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_FailsWithNullUserName()
    {
        // Arrange
        var dto = new ApplicationUserDto(Guid.NewGuid(), "test@example.com", null);

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_FailsWithShortUserName()
    {
        // Arrange
        var dto = new ApplicationUserDto(Guid.NewGuid(), "test@example.com", "ab");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validator_SucceedsWithMinimumLengthUserName()
    {
        // Arrange
        var dto = new ApplicationUserDto(Guid.NewGuid(), "test@example.com", "abc");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        Assert.True(result.IsValid);
    }
}

public class AppRolesTests
{
    [Fact]
    public void AppRoles_AdministratorConstantEquals_Administrator()
    {
        // Assert
        Assert.Equal("Administrator", AppRoles.Administrator);
    }

    [Fact]
    public void AppRoles_UserConstantEquals_User()
    {
        // Assert
        Assert.Equal("User", AppRoles.User);
    }

    [Fact]
    public void AppRoles_HasTwoRoles()
    {
        // Assert
        Assert.NotNull(AppRoles.Administrator);
        Assert.NotNull(AppRoles.User);
        Assert.NotEqual(AppRoles.Administrator, AppRoles.User);
    }
}

public class AppScopesTests
{
    [Fact]
    public void AppScopes_Services_PostsConstantEquals_services_posts()
    {
        // Assert
        Assert.Equal("services.posts", AppScopes.Services.Posts);
    }

    [Fact]
    public void AppScopes_PostsScopeIsValid()
    {
        // Assert
        Assert.NotNull(AppScopes.Services.Posts);
        Assert.Contains("services", AppScopes.Services.Posts);
        Assert.Contains("posts", AppScopes.Services.Posts);
    }
}
