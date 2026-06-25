namespace SupervisorioSIMMAQ_NXA.Dtos;

public class MqttTagPayload
{
    public string? Tag { get; set; }

    public string? Value { get; set; }

    public DateTime? Timestamp { get; set; }
}
