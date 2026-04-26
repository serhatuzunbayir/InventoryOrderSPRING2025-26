using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(UserService userService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile(int id)
    {
        // Users can only view their own profile
        if (!IsCurrentUser(id)) return Forbid();

        var user = await userService.GetProfileAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile(int id, UpdateUserRequest req)
    {
        if (!IsCurrentUser(id)) return Forbid();

        var user = await userService.UpdateProfileAsync(id, req);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpGet("{id}/addresses")]
    public async Task<IActionResult> GetAddresses(int id)
    {
        if (!IsCurrentUser(id)) return Forbid();
        return Ok(await userService.GetAddressesAsync(id));
    }

    [HttpPost("{id}/addresses")]
    public async Task<IActionResult> AddAddress(int id, AddressRequest req)
    {
        if (!IsCurrentUser(id)) return Forbid();

        var address = await userService.AddAddressAsync(id, req);
        return address == null ? NotFound() : Ok(address);
    }

    [HttpDelete("{id}/addresses/{addressId}")]
    public async Task<IActionResult> DeleteAddress(int id, int addressId)
    {
        if (!IsCurrentUser(id)) return Forbid();
        return await userService.DeleteAddressAsync(id, addressId) ? NoContent() : NotFound();
    }

    // Compares requested ID against the user ID from the JWT token
    private bool IsCurrentUser(int id)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return currentUserId == id.ToString();
    }
}
