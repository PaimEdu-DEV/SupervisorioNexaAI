using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupervisorioSIMMAQ_NXA.Data;
using SupervisorioSIMMAQ_NXA.Dtos;
using SupervisorioSIMMAQ_NXA.Models;

namespace SupervisorioSIMMAQ_NXA.Controllers;

[ApiController]
[Route("api/machine-readings")]
public class MachineReadingsController(SupervisorioDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MachineReading>>> GetAll(
        [FromQuery] string? machineCode,
        [FromQuery] string? tag,
        [FromQuery] int take = 100)
    {
        take = Math.Clamp(take, 1, 500);

        var query = dbContext.MachineReadings.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(machineCode))
        {
            query = query.Where(reading => reading.MachineCode == machineCode);
        }

        if (!string.IsNullOrWhiteSpace(tag))
        {
            query = query.Where(reading => reading.Tag == tag);
        }

        var readings = await query
            .OrderByDescending(reading => reading.CollectedAt)
            .Take(take)
            .ToListAsync();

        return Ok(readings);
    }

    [HttpGet("latest")]
    public async Task<ActionResult<IEnumerable<MachineReading>>> GetLatest()
    {
        var readings = await dbContext.MachineReadings
            .AsNoTracking()
            .OrderByDescending(reading => reading.CollectedAt)
            .ToListAsync();

        var latestReadings = readings
            .GroupBy(reading => new { reading.MachineCode, reading.Tag })
            .Select(group => group.First())
            .OrderBy(reading => reading.MachineCode)
            .ThenBy(reading => reading.Tag)
            .ToList();

        return Ok(latestReadings);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MachineReading>> GetById(int id)
    {
        var reading = await dbContext.MachineReadings.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

        return reading is null ? NotFound() : Ok(reading);
    }

    [HttpPost]
    public async Task<ActionResult<MachineReading>> Create(CreateMachineReadingRequest request)
    {
        var reading = new MachineReading
        {
            MachineCode = request.MachineCode,
            Tag = request.Tag,
            Value = request.Value,
            Unit = request.Unit,
            Status = request.Status,
            Source = "Manual",
            CollectedAt = request.CollectedAt ?? DateTime.UtcNow
        };

        dbContext.MachineReadings.Add(reading);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = reading.Id }, reading);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var reading = await dbContext.MachineReadings.FindAsync(id);

        if (reading is null)
        {
            return NotFound();
        }

        dbContext.MachineReadings.Remove(reading);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
