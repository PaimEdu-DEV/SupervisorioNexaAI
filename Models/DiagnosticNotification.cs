namespace SupervisorioSIMMAQ_NXA.Models;

public class DiagnosticNotification
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Severity { get; set; } = "Warning";

    public string Origin { get; set; } = "DiagnosticEngine";

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string PossibleCauses { get; set; } = string.Empty;

    public string RecommendedActions { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public string? SourceTag { get; set; }

    public double Confidence { get; set; } = 0.7;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ResolvedAt { get; set; }
}
