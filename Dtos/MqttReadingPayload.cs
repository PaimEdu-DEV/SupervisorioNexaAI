namespace SupervisorioSIMMAQ_NXA.Dtos;

public class MqttReadingPayload
{
    public string? MachineCode { get; set; }

    public string? Tag { get; set; }

    public double? Value { get; set; }

    public string? Unit { get; set; }

    public string? Status { get; set; }

    public DateTime? CollectedAt { get; set; }
}
