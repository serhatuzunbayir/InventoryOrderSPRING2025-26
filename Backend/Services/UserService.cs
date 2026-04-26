using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class UserService(AppDbContext db)
{
    public async Task<object?> GetProfileAsync(int id)
    {
        return await db.Users
            .Select(u => new { u.Id, u.Username, u.Email, u.PhoneNumber, u.FirstName, u.LastName, u.UserType })
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<object?> UpdateProfileAsync(int id, UpdateUserRequest req)
    {
        var user = await db.Users.FindAsync(id);
        if (user == null) return null;

        user.Email = req.Email;
        user.PhoneNumber = req.PhoneNumber;
        user.FirstName = req.FirstName;
        user.LastName = req.LastName;

        await db.SaveChangesAsync();
        return new { user.Id, user.Username, user.Email, user.FirstName, user.LastName };
    }

    public async Task<List<Address>> GetAddressesAsync(int id)
    {
        return await db.Addresses
            .Where(a => a.UserId == id)
            .ToListAsync();
    }

    public async Task<Address?> AddAddressAsync(int id, AddressRequest req)
    {
        var user = await db.Users.FindAsync(id);
        if (user == null) return null;

        var address = new Address
        {
            UserId = id,
            AddressName = req.AddressName,
            AddressLine = req.AddressLine
        };

        db.Addresses.Add(address);
        await db.SaveChangesAsync();
        return address;
    }

    public async Task<bool> DeleteAddressAsync(int id, int addressId)
    {
        var address = await db.Addresses
            .FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == id);

        if (address == null) return false;

        db.Addresses.Remove(address);
        await db.SaveChangesAsync();
        return true;
    }
}
