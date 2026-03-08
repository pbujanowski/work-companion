using Xunit;
using WorkCompanion.Identity.Domain.Entities;

namespace WorkCompanion.Identity.Domain.Tests;

public class ApplicationUserTests
{
    [Fact]
    public void ApplicationUser_CanBeCreated()
    {
        // Arrange & Act
        var user = new ApplicationUser();

        // Assert
        Assert.NotNull(user);
        Assert.IsType<ApplicationUser>(user);
    }

    [Fact]
    public void ApplicationUser_HasGuidId()
    {
        // Arrange
        var user = new ApplicationUser();

        // Act
        var id = user.Id;

        // Assert
        Assert.Equal(default(Guid), id);
    }

    [Fact]
    public void ApplicationUser_CanSetEmail()
    {
        // Arrange
        var user = new ApplicationUser();
        var email = "test@example.com";

        // Act
        user.Email = email;

        // Assert
        Assert.Equal(email, user.Email);
    }

    [Fact]
    public void ApplicationUser_CanSetUserName()
    {
        // Arrange
        var user = new ApplicationUser();
        var userName = "testuser";

        // Act
        user.UserName = userName;

        // Assert
        Assert.Equal(userName, user.UserName);
    }
}

public class ApplicationRoleTests
{
    [Fact]
    public void ApplicationRole_CanBeCreatedWithoutParameters()
    {
        // Arrange & Act
        var role = new ApplicationRole();

        // Assert
        Assert.NotNull(role);
        Assert.IsType<ApplicationRole>(role);
    }

    [Fact]
    public void ApplicationRole_CanBeCreatedWithName()
    {
        // Arrange
        var roleName = "Administrator";

        // Act
        var role = new ApplicationRole(roleName);

        // Assert
        Assert.NotNull(role);
        Assert.Equal(roleName, role.Name);
    }

    [Fact]
    public void ApplicationRole_HasGuidId()
    {
        // Arrange
        var role = new ApplicationRole();

        // Act
        var id = role.Id;

        // Assert
        Assert.Equal(default(Guid), id);
    }

    [Fact]
    public void ApplicationRole_CanSetName()
    {
        // Arrange
        var role = new ApplicationRole();
        var roleName = "User";

        // Act
        role.Name = roleName;

        // Assert
        Assert.Equal(roleName, role.Name);
    }

    [Fact]
    public void ApplicationRole_CanHaveNullName()
    {
        // Arrange & Act
        var role = new ApplicationRole();

        // Assert
        Assert.Null(role.Name);
    }
}
