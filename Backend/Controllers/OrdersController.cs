using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController(OrderService orderService) : ControllerBase
{
    // Staff sees all orders; Customer sees only their own
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role)!;
        return Ok(await orderService.GetAllAsync(userId, role));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role);

        var order = await orderService.GetByIdAsync(id);
        if (order == null) return NotFound();

        // Customer can only view their own order
        if (role == "Customer" && order.UserId != userId)
            return Forbid();

        return Ok(order);
    }

    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest req)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await orderService.CreateAsync(userId, req);
        if (!result.Success) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [Authorize(Roles = "Staff")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusRequest req)
    {
        var result = await orderService.UpdateStatusAsync(id, req);
        if (!result.Success)
            return result.Error == "Order not found." ? NotFound(result.Error) : BadRequest(result.Error);
        return Ok(result.Data);
    }
}
