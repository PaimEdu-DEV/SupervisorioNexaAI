namespace SupervisorioSIMMAQ_NXA.Models;

public class MaintenanceHistory
{
    public int Id { get; set; }

    public int? DiagnosticNotificationId { get; set; }

    public string FailureDescription { get; set; } = string.Empty;

    public string RealCause { get; set; } = string.Empty;

    public string Component { get; set; } = string.Empty;

    public string ActionTaken { get; set; } = string.Empty;

    public string RegisteredBy { get; set; } = "Operador";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
