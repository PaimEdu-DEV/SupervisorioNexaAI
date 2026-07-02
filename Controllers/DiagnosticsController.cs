using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupervisorioSIMMAQ_NXA.Data;
using SupervisorioSIMMAQ_NXA.Dtos;
using SupervisorioSIMMAQ_NXA.Models;

namespace SupervisorioSIMMAQ_NXA.Controllers;

[ApiController]
[Route("api/diagnostics")]
public class DiagnosticsController(SupervisorioDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DiagnosticNotification>>> GetDiagnostics(
        [FromQuery] string status = "Pending",
        [FromQuery] int take = 50)
    {
        take = Math.Clamp(take, 1, 200);

        var query = dbContext.DiagnosticNotifications.AsNoTracking();

        if (!status.Equals("All", StringComparison.OrdinalIgnoreCase))
        {
            query = query.Where(notification => notification.Status == status);
        }

        var notifications = await query
            .OrderByDescending(notification => notification.CreatedAt)
            .Take(take)
            .ToListAsync();

        return Ok(notifications);
    }

    [HttpPost("{id:int}/resolve")]
    public async Task<IActionResult> Resolve(int id, ResolveDiagnosticRequest request)
    {
        var notification = await dbContext.DiagnosticNotifications.FindAsync(id);

        if (notification is null)
        {
            return NotFound();
        }

        notification.Status = "Resolved";
        notification.ResolvedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(request.RealCause))
        {
            dbContext.MaintenanceHistory.Add(new MaintenanceHistory
            {
                DiagnosticNotificationId = notification.Id,
                FailureDescription = notification.Description,
                RealCause = request.RealCause,
                Component = request.Component,
                ActionTaken = request.ActionTaken,
                RegisteredBy = request.RegisteredBy,
                CreatedAt = DateTime.UtcNow
            });
        }

        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("maintenance-history")]
    public async Task<ActionResult<IEnumerable<MaintenanceHistory>>> GetMaintenanceHistory([FromQuery] int take = 100)
    {
        take = Math.Clamp(take, 1, 300);

        var history = await dbContext.MaintenanceHistory
            .AsNoTracking()
            .OrderByDescending(item => item.CreatedAt)
            .Take(take)
            .ToListAsync();

        return Ok(history);
    }
}
