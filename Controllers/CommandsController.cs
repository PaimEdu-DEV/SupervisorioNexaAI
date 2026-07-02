using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupervisorioSIMMAQ_NXA.Data;
using SupervisorioSIMMAQ_NXA.Dtos;
using SupervisorioSIMMAQ_NXA.Models;
using SupervisorioSIMMAQ_NXA.Services;

namespace SupervisorioSIMMAQ_NXA.Controllers;

[ApiController]
[Route("api/commands")]
public class CommandsController(SupervisorioDbContext dbContext, TagValueService tagValueService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommandExecution>>> GetHistory([FromQuery] int take = 100)
    {
        take = Math.Clamp(take, 1, 500);

        var commands = await dbContext.CommandExecutions
            .AsNoTracking()
            .OrderByDescending(command => command.CreatedAt)
            .Take(take)
            .ToListAsync();

        return Ok(commands);
    }

    [HttpPost]
    public async Task<ActionResult<CommandExecution>> Execute(ExecuteCommandRequest request)
    {
        var commandTag = await dbContext.IndustrialTags.FirstOrDefaultAsync(tag => tag.Name == request.CommandTag);

        if (commandTag is null)
        {
            return NotFound($"Tag de comando '{request.CommandTag}' nao cadastrada.");
        }

        if (!commandTag.IsCommand)
        {
            return BadRequest($"A tag '{request.CommandTag}' nao esta marcada como comando.");
        }

        await tagValueService.UpdateTagAsync(request.CommandTag, request.Value, "Command");

        var command = new CommandExecution
        {
            CommandTag = request.CommandTag,
            Value = request.Value,
            RequestedBy = request.RequestedBy,
            Status = "Requested",
            CreatedAt = DateTime.UtcNow
        };

        dbContext.CommandExecutions.Add(command);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetHistory), new { id = command.Id }, command);
    }
}
