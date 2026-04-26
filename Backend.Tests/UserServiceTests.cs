using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Xunit;

namespace Backend.Tests;

public class UserServiceTests : ServiceTestBase
{
    private readonly UserService _service;

    public UserServiceTests()
    {
        _service = new UserService(Db);
    }

    private async Task<User> SeedUser(string username = "testuser")
    {
        var user = new User
        {
            Username = username,
            PasswordHash = "hash",
            Email = $"{username}@test.com",
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "123",
            UserType = "Customer"
        };
        Db.Users.Add(user);
        await Db.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task GetProfile_ExistingUser_ReturnsProfile()
    {
        var user = await SeedUser();
        var result = await _service.GetProfileAsync(user.Id);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetProfile_NonExistingUser_ReturnsNull()
    {
        var result = await _service.GetProfileAsync(999);
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateProfile_ExistingUser_UpdatesAndReturns()
    {
        var user = await SeedUser();
        var result = await _service.UpdateProfileAsync(user.Id, new UpdateUserRequest
        {
            Email = "new@example.com",
            PhoneNumber = "999",
            FirstName = "Updated",
            LastName = "Name"
        });
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateProfile_NonExistingUser_ReturnsNull()
    {
        var result = await _service.UpdateProfileAsync(999, new UpdateUserRequest
        {
            Email = "x@x.com",
            PhoneNumber = "0",
            FirstName = "X",
            LastName = "Y"
        });
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAddresses_ReturnsUserAddresses()
    {
        var user = await SeedUser();
        await _service.AddAddressAsync(user.Id, new AddressRequest { AddressName = "Home", AddressLine = "123 Main St" });
        await _service.AddAddressAsync(user.Id, new AddressRequest { AddressName = "Work", AddressLine = "456 Office Blvd" });

        var addresses = await _service.GetAddressesAsync(user.Id);
        Assert.Equal(2, addresses.Count);
    }

    [Fact]
    public async Task AddAddress_ExistingUser_ReturnsAddress()
    {
        var user = await SeedUser();
        var address = await _service.AddAddressAsync(user.Id, new AddressRequest { AddressName = "Home", AddressLine = "123 St" });
        Assert.NotNull(address);
        Assert.Equal("Home", address.AddressName);
    }

    [Fact]
    public async Task AddAddress_NonExistingUser_ReturnsNull()
    {
        var address = await _service.AddAddressAsync(999, new AddressRequest { AddressName = "Home", AddressLine = "123 St" });
        Assert.Null(address);
    }

    [Fact]
    public async Task DeleteAddress_ExistingAddress_ReturnsTrue()
    {
        var user = await SeedUser();
        var address = await _service.AddAddressAsync(user.Id, new AddressRequest { AddressName = "Home", AddressLine = "123 St" });
        Assert.True(await _service.DeleteAddressAsync(user.Id, address!.Id));
    }

    [Fact]
    public async Task DeleteAddress_WrongUser_ReturnsFalse()
    {
        var user1 = await SeedUser("user1");
        var user2 = await SeedUser("user2");
        var address = await _service.AddAddressAsync(user1.Id, new AddressRequest { AddressName = "Home", AddressLine = "123 St" });
        Assert.False(await _service.DeleteAddressAsync(user2.Id, address!.Id));
    }
}
