using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Entity;
using MyApp.Namespace;

namespace API.Tests;

public class UserControllerTests
{
    private static ApplicationDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetUsers_ReturnsEmptyList_WhenNoUsersExist()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var controller = new UserController(context);

        // Act
        var result = await controller.GetUsers();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<User>>>(result);
        var users = Assert.IsAssignableFrom<IEnumerable<User>>(actionResult.Value);
        Assert.Empty(users);
    }

    [Fact]
    public async Task GetUsers_ReturnsAllUsers_WhenUsersExist()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        context.Users.AddRange(
            new User { Id = 1, Name = "Alice", Email = "alice@example.com", CreatedAt = DateTime.UtcNow },
            new User { Id = 2, Name = "Bob", Email = "bob@example.com", CreatedAt = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var controller = new UserController(context);

        // Act
        var result = await controller.GetUsers();

        // Assert
        var users = Assert.IsAssignableFrom<IEnumerable<User>>(result.Value);
        Assert.Equal(2, users.Count());
    }

    [Fact]
    public async Task GetUser_ReturnsUser_WhenUserExists()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var user = new User { Id = 1, Name = "Alice", Email = "alice@example.com", CreatedAt = DateTime.UtcNow };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var controller = new UserController(context);

        // Act
        var result = await controller.GetUser(1);

        // Assert
        var returnedUser = Assert.IsType<User>(result.Value);
        Assert.Equal("Alice", returnedUser.Name);
        Assert.Equal("alice@example.com", returnedUser.Email);
    }

    [Fact]
    public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var controller = new UserController(context);

        // Act
        var result = await controller.GetUser(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedAtAction_WithNewUser()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var controller = new UserController(context);
        var newUser = new User { Name = "Charlie", Email = "charlie@example.com", CreatedAt = DateTime.UtcNow };

        // Act
        var result = await controller.CreateUser(newUser);

        // Assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedUser = Assert.IsType<User>(createdAtResult.Value);
        Assert.Equal("Charlie", returnedUser.Name);
        Assert.True(returnedUser.Id > 0);
    }

    [Fact]
    public async Task CreateUser_AddsUserToDatabase()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var controller = new UserController(context);
        var newUser = new User { Name = "Diana", Email = "diana@example.com", CreatedAt = DateTime.UtcNow };

        // Act
        await controller.CreateUser(newUser);

        // Assert
        var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Name == "Diana");
        Assert.NotNull(userInDb);
        Assert.Equal("diana@example.com", userInDb.Email);
    }
}
