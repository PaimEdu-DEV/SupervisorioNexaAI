using Microsoft.EntityFrameworkCore;
using SupervisorioSIMMAQ_NXA.Data;
using SupervisorioSIMMAQ_NXA.Models;

namespace SupervisorioSIMMAQ_NXA.Services;

public class TagValueService(SupervisorioDbContext dbContext)
{
    public async Task<IndustrialTag> UpdateTagAsync(
        string tagName,
        string value,
        string source,
        string? mqttTopic = null,
        string? rawPayload = null,
        DateTime? timestamp = null,
        CancellationToken cancellationToken = default)
    {
        var tag = await dbContext.IndustrialTags.FirstOrDefaultAsync(item => item.Name == tagName, cancellationToken);

        if (tag is null)
        {
            tag = new IndustrialTag
            {
                Name = tagName,
                DisplayName = tagName,
                Category = tagName.Split('.').FirstOrDefault() ?? "Generic",
                DataType = "String",
                CurrentValue = value,
                UpdatedAt = timestamp ?? DateTime.UtcNow
            };

            dbContext.IndustrialTags.Add(tag);
        }
        else
        {
            tag.CurrentValue = value;
            tag.UpdatedAt = timestamp ?? DateTime.UtcNow;
        }

        dbContext.TagValueHistory.Add(new TagValueHistory
        {
            TagName = tagName,
            Value = value,
            Source = source,
            MqttTopic = mqttTopic,
            RawPayload = rawPayload,
            CreatedAt = timestamp ?? DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        return tag;
    }
}
