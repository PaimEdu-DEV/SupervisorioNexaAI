namespace SupervisorioSIMMAQ_NXA.Models;

public class MachineReading
{
    public int Id { get; set; }

    public string MachineCode { get; set; } = string.Empty;

    public string Tag { get; set; } = string.Empty;

    public double Value { get; set; }

    public string Unit { get; set; } = string.Empty;

    public string Status { get; set; } = "Normal";

    public string Source { get; set; } = "Manual";

    public string? MqttTopic { get; set; }

    public string? RawPayload { get; set; }

    public DateTime CollectedAt { get; set; } = DateTime.UtcNow;
}
