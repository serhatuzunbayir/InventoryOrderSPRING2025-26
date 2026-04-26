using Backend.DTOs;
using Backend.Services;
using Xunit;

namespace Backend.Tests;

public class ItemServiceTests : ServiceTestBase
{
    private readonly ItemService _service;

    public ItemServiceTests()
    {
        _service = new ItemService(Db);
    }

    private Task<Backend.Models.Item> Seed(string name = "Widget", string category = "Electronics", double price = 9.99, int quantity = 10)
        => _service.CreateAsync(new ItemRequest { Name = name, Category = category, Price = price, Quantity = quantity });

    [Fact]
    public async Task GetAll_NoFilter_ReturnsAllItems()
    {
        await Seed("Apple");
        await Seed("Banana");
        var result = await _service.GetAllAsync(null, null, null);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAll_FilterByName_ReturnsMatching()
    {
        await Seed("Apple");
        await Seed("Banana");
        var result = await _service.GetAllAsync("App", null, null);
        Assert.Single(result);
        Assert.Equal("Apple", result[0].Name);
    }

    [Fact]
    public async Task GetAll_FilterByCategory_ReturnsMatching()
    {
        await Seed("Apple", category: "Fruit");
        await Seed("Phone", category: "Electronics");
        var result = await _service.GetAllAsync(null, "Fruit", null);
        Assert.Single(result);
        Assert.Equal("Fruit", result[0].Category);
    }

    [Fact]
    public async Task GetAll_SortByPriceAsc_ReturnsOrdered()
    {
        await Seed("Cheap", price: 1.0);
        await Seed("Expensive", price: 100.0);
        await Seed("Mid", price: 50.0);
        var result = await _service.GetAllAsync(null, null, "asc");
        Assert.Equal(1.0, result[0].Price);
        Assert.Equal(100.0, result[2].Price);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsItem()
    {
        var item = await Seed();
        var result = await _service.GetByIdAsync(item.Id);
        Assert.NotNull(result);
        Assert.Equal(item.Id, result.Id);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNull()
    {
        var result = await _service.GetByIdAsync(999);
        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ValidRequest_ReturnsCreatedItem()
    {
        var item = await _service.CreateAsync(new ItemRequest { Name = "New Item", Category = "Test", Price = 5.0, Quantity = 3 });
        Assert.NotEqual(0, item.Id);
        Assert.Equal("New Item", item.Name);
    }

    [Fact]
    public async Task Update_ExistingId_UpdatesAndReturns()
    {
        var item = await Seed("Old Name");
        var updated = await _service.UpdateAsync(item.Id, new ItemRequest { Name = "New Name", Category = "Cat", Price = 2.0, Quantity = 5 });
        Assert.NotNull(updated);
        Assert.Equal("New Name", updated.Name);
    }

    [Fact]
    public async Task Update_NonExistingId_ReturnsNull()
    {
        var result = await _service.UpdateAsync(999, new ItemRequest { Name = "X", Category = "Y", Price = 1.0, Quantity = 1 });
        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_ExistingId_ReturnsTrue()
    {
        var item = await Seed();
        Assert.True(await _service.DeleteAsync(item.Id));
    }

    [Fact]
    public async Task Delete_NonExistingId_ReturnsFalse()
    {
        Assert.False(await _service.DeleteAsync(999));
    }
}
