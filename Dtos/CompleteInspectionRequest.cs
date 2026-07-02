namespace SupervisorioSIMMAQ_NXA.Dtos;

public class CompleteInspectionRequest
{
    public string ComponentId { get; set; } = string.Empty;

    public string FailureDescription { get; set; } = string.Empty;

    public string RealCause { get; set; } = string.Empty;

    public string ActionTaken { get; set; } = string.Empty;

    public string RegisteredBy { get; set; } = "Operador";
}
