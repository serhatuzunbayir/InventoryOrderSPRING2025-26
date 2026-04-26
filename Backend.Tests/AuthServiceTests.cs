using Backend.DTOs.Auth;
using Backend.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Backend.Tests;

public class AuthServiceTests : ServiceTestBase
{
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["Jwt:Key"] = "test-secret-key-that-is-at-least-32-chars-long!" })
            .Build();
        _service = new AuthService(Db, config);
    }

    private RegisterRequest NewUser(string username, string email) => new()
    {
        Username = username,
        Password = "password123",
        Email = email,
        FirstName = "Test",
        LastName = "User",
        PhoneNumber = "1234567890",
        UserType = "Customer"
    };

    [Fact]
    public async Task Register_NewUser_ReturnsSuccess()
    {
        var result = await _service.RegisterAsync(NewUser("alice", "alice@example.com"));
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task Register_DuplicateUsername_ReturnsFail()
    {
        await _service.RegisterAsync(NewUser("bob", "bob@example.com"));
        var result = await _service.RegisterAsync(NewUser("bob", "other@example.com"));
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task Register_DuplicateEmail_ReturnsFail()
    {
        await _service.RegisterAsync(NewUser("user1", "shared@example.com"));
        var result = await _service.RegisterAsync(NewUser("user2", "shared@example.com"));
        Assert.False(result.Success);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        await _service.RegisterAsync(NewUser("charlie", "charlie@example.com"));
        var result = await _service.LoginAsync(new LoginRequest { Username = "charlie", Password = "password123" });
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task Login_WrongPassword_ReturnsFail()
    {
        await _service.RegisterAsync(NewUser("dave", "dave@example.com"));
        var result = await _service.LoginAsync(new LoginRequest { Username = "dave", Password = "wrongpassword" });
        Assert.False(result.Success);
    }

    [Fact]
    public async Task Login_UnknownUser_ReturnsFail()
    {
        var result = await _service.LoginAsync(new LoginRequest { Username = "nonexistent", Password = "anypassword" });
        Assert.False(result.Success);
    }
}
