using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupervisorioSIMMAQ_NXA.Data;
using SupervisorioSIMMAQ_NXA.Models;

namespace SupervisorioSIMMAQ_NXA.Controllers;

[ApiController]
[Route("api/history")]
public class HistoryController(SupervisorioDbContext dbContext) : ControllerBase
{
    [HttpGet("tags")]
    public async Task<ActionResult<IEnumerable<TagValueHistory>>> GetTagHistory(
        [FromQuery] string? tagName,
        [FromQuery] string? category,
        [FromQuery] int take = 200)
    {
        take = Math.Clamp(take, 1, 1000);

        var query = dbContext.TagValueHistory.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(tagName))
        {
            query = query.Where(history => history.TagName == tagName);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(history => history.TagName.StartsWith(category + "."));
        }

        var historyItems = await query
            .OrderByDescending(history => history.CreatedAt)
            .Take(take)
            .ToListAsync();

        return Ok(historyItems);
    }
}
