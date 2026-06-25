namespace SupervisorioSIMMAQ_NXA.Models;

public class TagValueHistory
{
    public int Id { get; set; }

    public string TagName { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string Source { get; set; } = "Manual";

    public string? MqttTopic { get; set; }

    public string? RawPayload { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
