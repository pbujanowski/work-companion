using Xunit;
using WorkCompanion.Identity.Infrastructure.Services;
using WorkCompanion.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WorkCompanion.Identity.Infrastructure.Data;

namespace WorkCompanion.Identity.Infrastructure.Tests;

public class ApplicationUserServiceTests : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ApplicationUserService _service;

    public ApplicationUserServiceTests()
    {
        // Setup in-memory database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _service = new ApplicationUserService(_dbContext);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsUserDto_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new ApplicationUser
        {
            Id = userId,
            Email = "test@example.com",
            UserName = "testuser"
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("test@example.com", result.Email);
        Assert.Equal("testuser", result.UserName);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsNull_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await _service.GetUserByIdAsync(userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsNull_WhenCalledWithEmptyGuid()
    {
        // Act
        var result = await _service.GetUserByIdAsync(Guid.Empty);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsCorrectData_ForMultipleUsers()
    {
        // Arrange
        var user1Id = Guid.NewGuid();
        var user2Id = Guid.NewGuid();

        _dbContext.Users.AddRange(
            new ApplicationUser { Id = user1Id, Email = "user1@example.com", UserName = "user1" },
            new ApplicationUser { Id = user2Id, Email = "user2@example.com", UserName = "user2" }
        );
        await _dbContext.SaveChangesAsync();

        // Act
        var result1 = await _service.GetUserByIdAsync(user1Id);
        var result2 = await _service.GetUserByIdAsync(user2Id);

        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.Equal("user1@example.com", result1.Email);
        Assert.Equal("user2@example.com", result2.Email);
        Assert.NotEqual(result1.Id, result2.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_DoesNotModifyDatabase()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new ApplicationUser
        {
            Id = userId,
            Email = "test@example.com",
            UserName = "testuser"
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var initialCount = _dbContext.Users.Count();

        // Act
        var result = await _service.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(initialCount, _dbContext.Users.Count());
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }
}
