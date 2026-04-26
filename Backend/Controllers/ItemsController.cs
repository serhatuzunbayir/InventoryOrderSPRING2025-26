using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController(ItemService itemService) : ControllerBase
{
    // Public: list, filter, sort items
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? name,
        [FromQuery] string? category,
        [FromQuery] string? sortByPrice)
    {
        return Ok(await itemService.GetAllAsync(name, category, sortByPrice));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await itemService.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    // Only Staff can create, update, or delete items
    [Authorize(Roles = "Staff")]
    [HttpPost]
    public async Task<IActionResult> Create(ItemRequest req)
    {
        var item = await itemService.CreateAsync(req);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [Authorize(Roles = "Staff")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ItemRequest req)
    {
        var item = await itemService.UpdateAsync(id, req);
        return item == null ? NotFound() : Ok(item);
    }

    [Authorize(Roles = "Staff")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await itemService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
