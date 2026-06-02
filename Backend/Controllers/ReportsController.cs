using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Staff")]
public class ReportsController(ReportsService reportsService) : ControllerBase
{
    [HttpGet("sales")]
    public async Task<IActionResult> GetSalesReport()
    {
        var report = await reportsService.GetSalesReportAsync();
        return Ok(report);
    }

    [HttpGet("item-trends/{itemId:int}")]
    public async Task<IActionResult> GetItemTrendReport(int itemId)
    {
        var report = await reportsService.GetItemTrendReportAsync(itemId);
        return report == null ? NotFound() : Ok(report);
    }
}
