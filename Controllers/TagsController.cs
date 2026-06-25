using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupervisorioSIMMAQ_NXA.Data;
using SupervisorioSIMMAQ_NXA.Dtos;
using SupervisorioSIMMAQ_NXA.Models;
using SupervisorioSIMMAQ_NXA.Services;

namespace SupervisorioSIMMAQ_NXA.Controllers;

[ApiController]
[Route("api/tags")]
public class TagsController(SupervisorioDbContext dbContext, TagValueService tagValueService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IndustrialTag>>> GetTags([FromQuery] string? category)
    {
        var query = dbContext.IndustrialTags.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(tag => tag.Category == category);
        }

        var tags = await query
            .OrderBy(tag => tag.Category)
            .ThenBy(tag => tag.Name)
            .ToListAsync();

        return Ok(tags);
    }

    [HttpGet("current")]
    public async Task<ActionResult<object>> GetCurrentValues()
    {
        var tags = await dbContext.IndustrialTags
            .AsNoTracking()
            .OrderBy(tag => tag.Category)
            .ThenBy(tag => tag.Name)
            .ToListAsync();

        var groupedTags = tags
            .GroupBy(tag => tag.Category)
            .ToDictionary(group => group.Key, group => group.ToList());

        return Ok(groupedTags);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<IndustrialTag>> GetByName(string name)
    {
        var tag = await dbContext.IndustrialTags.AsNoTracking().FirstOrDefaultAsync(item => item.Name == name);
        return tag is null ? NotFound() : Ok(tag);
    }

    [HttpPut("{name}/value")]
    public async Task<ActionResult<IndustrialTag>> UpdateValue(string name, UpdateTagValueRequest request)
    {
        var tag = await tagValueService.UpdateTagAsync(name, request.Value, request.Source);
        return Ok(tag);
    }
}
