using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class ItemService(AppDbContext db)
{
    public async Task<List<Item>> GetAllAsync(string? name, string? category, string? sortByPrice)
    {
        var query = db.Items.AsQueryable();

        if (!string.IsNullOrEmpty(name))
            // EF.Functions.Like uses SQLite LIKE — case-insensitive
            query = query.Where(i => EF.Functions.Like(i.Name, $"%{name}%"));

        if (!string.IsNullOrEmpty(category))
            query = query.Where(i => i.Category == category);

        if (sortByPrice == "asc")
            query = query.OrderBy(i => i.Price);
        else if (sortByPrice == "desc")
            query = query.OrderByDescending(i => i.Price);

        return await query.ToListAsync();
    }

    public async Task<Item?> GetByIdAsync(int id) => await db.Items.FindAsync(id);

    public async Task<Item> CreateAsync(ItemRequest req)
    {
        var item = new Item
        {
            Name = req.Name,
            Quantity = req.Quantity,
            Price = req.Price,
            Category = req.Category
        };

        db.Items.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<Item?> UpdateAsync(int id, ItemRequest req)
    {
        var item = await db.Items.FindAsync(id);
        if (item == null) return null;

        item.Name = req.Name;
        item.Quantity = req.Quantity;
        item.Price = req.Price;
        item.Category = req.Category;

        await db.SaveChangesAsync();
        return item;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await db.Items.FindAsync(id);
        if (item == null) return false;

        db.Items.Remove(item);
        await db.SaveChangesAsync();
        return true;
    }
}
