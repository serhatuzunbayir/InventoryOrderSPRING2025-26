using Backend.Data;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Staff")]
public class BackupController(AppDbContext db) : ControllerBase
{
    // Endpoint the frontend calls to trigger a manual backup.
    [HttpPost]
    public IActionResult CreateBackup()
    {
        var path = BackupService.TakeBackup(db);
        return Ok(new { message = "Backup created.", file = Path.GetFileName(path) });
    }
}
