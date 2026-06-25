namespace SupervisorioSIMMAQ_NXA.Models;

public class CommandExecution
{
    public int Id { get; set; }

    public string CommandTag { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string RequestedBy { get; set; } = "Postman";

    public string Status { get; set; } = "Requested";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
