using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupervisorioSIMMAQ_NXA.Data;

namespace SupervisorioSIMMAQ_NXA.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController(SupervisorioDbContext dbContext) : ControllerBase
{
    [HttpGet("overview")]
    public async Task<ActionResult<object>> GetOverview()
    {
        var tags = await dbContext.IndustrialTags.AsNoTracking().ToListAsync();
        var tagMap = tags.ToDictionary(tag => tag.Name, tag => tag.CurrentValue);

        var activeAlarms = tags
            .Where(tag => tag.Category == "Alarms" && IsTrue(tag.CurrentValue))
            .OrderBy(tag => tag.DisplayName)
            .ToList();
        var pendingDiagnostics = await dbContext.DiagnosticNotifications
            .AsNoTracking()
            .Where(notification => notification.Status == "Pending")
            .OrderByDescending(notification => notification.CreatedAt)
            .Take(5)
            .ToListAsync();

        return Ok(new
        {
            machine = new
            {
                status = ValueOf(tagMap, "Machine.Status"),
                mode = ValueOf(tagMap, "Machine.Mode"),
                cycleTimeCurrent = ValueOf(tagMap, "Machine.CycleTimeCurrent"),
                dateTime = DateTime.Now
            },
            communication = new
            {
                plcConnected = IsTrue(ValueOf(tagMap, "Communication.PlcConnected")),
                mqttConnected = IsTrue(ValueOf(tagMap, "Communication.MqttConnected"))
            },
            production = new
            {
                totalProcessed = ToNumber(ValueOf(tagMap, "Counters.TotalProcessed")),
                totalApproved = ToNumber(ValueOf(tagMap, "Counters.TotalApproved")),
                totalRejected = ToNumber(ValueOf(tagMap, "Counters.TotalRejected")),
                averageCycleTime = ToNumber(ValueOf(tagMap, "Counters.AverageCycleTime")),
                lastProcessedPart = ValueOf(tagMap, "Counters.LastProcessedPart"),
                machineEfficiency = ToNumber(ValueOf(tagMap, "Counters.MachineEfficiency"))
            },
            activeAlarms,
            pendingDiagnostics,
            sensors = tags.Where(tag => tag.Category == "Sensors").OrderBy(tag => tag.Name),
            actuators = tags.Where(tag => tag.Category == "Actuators").OrderBy(tag => tag.Name)
        });
    }

    private static string ValueOf(Dictionary<string, string> tagMap, string tagName) =>
        tagMap.TryGetValue(tagName, out var value) ? value : string.Empty;

    private static bool IsTrue(string value) =>
        value.Equals("true", StringComparison.OrdinalIgnoreCase) ||
        value.Equals("1", StringComparison.OrdinalIgnoreCase) ||
        value.Equals("on", StringComparison.OrdinalIgnoreCase) ||
        value.Equals("ligado", StringComparison.OrdinalIgnoreCase);

    private static double ToNumber(string value) =>
        double.TryParse(value, out var number) ? number : 0;
}
