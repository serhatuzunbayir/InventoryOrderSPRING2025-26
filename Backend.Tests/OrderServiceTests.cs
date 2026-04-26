using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Xunit;

namespace Backend.Tests;

public class OrderServiceTests : ServiceTestBase
{
    private readonly OrderService _service;
    private readonly ItemService _itemService;

    public OrderServiceTests()
    {
        _service = new OrderService(Db);
        _itemService = new ItemService(Db);
    }

    private async Task<User> SeedUser(string username = "customer1")
    {
        var user = new User
        {
            Username = username,
            PasswordHash = "hash",
            Email = $"{username}@test.com",
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "000",
            UserType = "Customer"
        };
        Db.Users.Add(user);
        await Db.SaveChangesAsync();
        return user;
    }

    private Task<Item> SeedItem(int quantity = 10, double price = 5.0)
        => _itemService.CreateAsync(new ItemRequest { Name = "Widget", Quantity = quantity, Price = price, Category = "Test" });

    [Fact]
    public async Task Create_ValidOrder_DeductsStockAndReturnsOrder()
    {
        var user = await SeedUser();
        var item = await SeedItem(quantity: 10);

        var result = await _service.CreateAsync(user.Id, new CreateOrderRequest
        {
            Items = [new OrderItemRequest { ItemId = item.Id, Quantity = 3 }]
        });

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        var dbItem = await Db.Items.FindAsync(item.Id);
        Assert.Equal(7, dbItem!.Quantity);
    }

    [Fact]
    public async Task Create_InsufficientStock_ReturnsFail()
    {
        var user = await SeedUser();
        var item = await SeedItem(quantity: 2);

        var result = await _service.CreateAsync(user.Id, new CreateOrderRequest
        {
            Items = [new OrderItemRequest { ItemId = item.Id, Quantity = 5 }]
        });

        Assert.False(result.Success);
        Assert.Contains("Insufficient stock", result.Error);
    }

    [Fact]
    public async Task Create_NonExistingItem_ReturnsFail()
    {
        var user = await SeedUser();

        var result = await _service.CreateAsync(user.Id, new CreateOrderRequest
        {
            Items = [new OrderItemRequest { ItemId = 9999, Quantity = 1 }]
        });

        Assert.False(result.Success);
        Assert.Contains("not found", result.Error);
    }

    [Fact]
    public async Task UpdateStatus_ValidStatus_UpdatesOrder()
    {
        var user = await SeedUser();
        var item = await SeedItem();
        var created = await _service.CreateAsync(user.Id, new CreateOrderRequest
        {
            Items = [new OrderItemRequest { ItemId = item.Id, Quantity = 1 }]
        });

        var result = await _service.UpdateStatusAsync(created.Data!.Id, new UpdateOrderStatusRequest { Status = "Processing" });

        Assert.True(result.Success);
        Assert.Equal("Processing", result.Data!.Status);
    }

    [Fact]
    public async Task UpdateStatus_InvalidStatus_ReturnsFail()
    {
        var user = await SeedUser();
        var item = await SeedItem();
        var created = await _service.CreateAsync(user.Id, new CreateOrderRequest
        {
            Items = [new OrderItemRequest { ItemId = item.Id, Quantity = 1 }]
        });

        var result = await _service.UpdateStatusAsync(created.Data!.Id, new UpdateOrderStatusRequest { Status = "Flying" });

        Assert.False(result.Success);
        Assert.Equal("Invalid status value.", result.Error);
    }

    [Fact]
    public async Task UpdateStatus_NonExistingOrder_ReturnsFail()
    {
        var result = await _service.UpdateStatusAsync(9999, new UpdateOrderStatusRequest { Status = "Shipped" });
        Assert.False(result.Success);
        Assert.Equal("Order not found.", result.Error);
    }

    [Fact]
    public async Task GetAll_CustomerRole_ReturnsOnlyOwnOrders()
    {
        var user1 = await SeedUser("user1");
        var user2 = await SeedUser("user2");
        var item = await SeedItem(quantity: 20);

        await _service.CreateAsync(user1.Id, new CreateOrderRequest { Items = [new OrderItemRequest { ItemId = item.Id, Quantity = 1 }] });
        await _service.CreateAsync(user2.Id, new CreateOrderRequest { Items = [new OrderItemRequest { ItemId = item.Id, Quantity = 1 }] });

        var orders = await _service.GetAllAsync(user1.Id, "Customer");
        Assert.Single(orders);
        Assert.All(orders, o => Assert.Equal(user1.Id, o.UserId));
    }

    [Fact]
    public async Task GetAll_StaffRole_ReturnsAllOrders()
    {
        var user1 = await SeedUser("userA");
        var user2 = await SeedUser("userB");
        var item = await SeedItem(quantity: 20);

        await _service.CreateAsync(user1.Id, new CreateOrderRequest { Items = [new OrderItemRequest { ItemId = item.Id, Quantity = 1 }] });
        await _service.CreateAsync(user2.Id, new CreateOrderRequest { Items = [new OrderItemRequest { ItemId = item.Id, Quantity = 1 }] });

        var orders = await _service.GetAllAsync(0, "Staff");
        Assert.Equal(2, orders.Count);
    }
}
